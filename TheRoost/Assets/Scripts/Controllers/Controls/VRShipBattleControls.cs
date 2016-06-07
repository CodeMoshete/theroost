using UnityEngine;
using System.Collections;
using Models;
using Services;
using Utils;

namespace Controllers.Controls
{
	public class VRShipBattleControls
	{
		private readonly Vector3 SHIP_GRAB_POS = new Vector3 (0f, -0.125f, 0.235f);
		private readonly Quaternion SHIP_GRAB_ROT = 
			new Quaternion (0.004537996f, -0.9590314f, -0.2831307f, 0.008671289f);

		private VRInteractionControls controls;
		private GameObject leftController;
		private GameObject rightController;
		private GameObject shipController;
		private GameObject vrRig;
		private ShipEntity localShip;
		private GameObject spaceDust;
		private bool shipIsGrabbed;

		private float lastScrollYVal;
		private float currentVelocityMult;

		public VRShipBattleControls(ShipEntity localShip)
		{
			controls = new VRInteractionControls ();
			this.localShip = localShip;
			spaceDust = GameObject.Instantiate (Resources.Load<GameObject>("Models/DustParticles"));
			controls.RegisterOnPress (localShip.Model, OnGrabShip);

			vrRig = GameObject.Find ("[CameraRig]");
			if (vrRig != null && vrRig.activeSelf)
			{
				leftController = GameObject.Find ("Controller (left)");
				rightController = GameObject.Find ("Controller (right)");
				spaceDust.transform.SetParent (vrRig.transform);
				spaceDust.transform.position = Vector3.zero;
			}
			else
			{
				GameObject debugCamera = GameObject.Find ("DebugCamera");
				if (debugCamera != null)
				{
					spaceDust.transform.SetParent (debugCamera.transform);
					spaceDust.transform.position = Vector3.zero;
				}
			}
		}

		public void Unload()
		{
			controls.Unload ();
		}

		public void Update()
		{
			if (shipIsGrabbed)
			{
				Vector3 moveDirection = localShip.Model.transform.forward * currentVelocityMult;
				if (vrRig != null)
				{
					vrRig.transform.Translate (moveDirection);
				}
				else
				{
					GameObject debugCamera = GameObject.Find ("DebugCamera");
					Vector3 pos = debugCamera.transform.position;
					pos += moveDirection;
					debugCamera.transform.position = pos;
				}
				Service.Network.BroadcastCurrentTransform (localShip);
			}
		}

		public void OnGrabShip()
		{
			if (vrRig != null && vrRig.activeSelf)
			{
				float lDist = 
					Vector3.SqrMagnitude (leftController.transform.position - localShip.Model.transform.position);
				float rDist = 
					Vector3.SqrMagnitude (rightController.transform.position - localShip.Model.transform.position);

				shipController = lDist < rDist ? leftController : rightController;
				localShip.Model.transform.SetParent (shipController.transform);
				localShip.Model.transform.localPosition = SHIP_GRAB_POS;
				localShip.Model.transform.localRotation = SHIP_GRAB_ROT;
				shipIsGrabbed = true;

				controls.RegisterOnTouchPress (shipController, OnAccelerateTouch);
				controls.RegisterOnTouchDrag (shipController, OnAccelerateDrag);

				UnityUtils.FindGameObject (shipController, "Model").SetActive (false);
			}
			else
			{
				GameObject debugCamera = GameObject.Find ("DebugCamera");
				if (debugCamera != null)
				{
					localShip.Model.transform.SetParent (debugCamera.transform);
					Vector3 startPos = new Vector3 (0f, -0.1f, 1f);
					Vector3 startEuler = new Vector3 (0f, 180f, 0f);
					localShip.Model.transform.localPosition = startPos;
					localShip.Model.transform.localEulerAngles = startEuler;
					controls.RegisterOnTouchPress (debugCamera, OnAccelerateTouch);
					controls.RegisterOnTouchDrag (debugCamera, OnAccelerateDrag);
					shipIsGrabbed = true;
				}
			}
		}

		private void OnAccelerateTouch(Vector2 touchPos)
		{
			lastScrollYVal = touchPos.y;
		}

		private void OnAccelerateDrag(Vector2 touchPos)
		{
			currentVelocityMult -= ((touchPos.y - lastScrollYVal) / 1) * localShip.Ship.MoveSpeed;
			lastScrollYVal = touchPos.y;
			if (currentVelocityMult > localShip.Ship.MoveSpeed)
			{
				currentVelocityMult = localShip.Ship.MoveSpeed;
			}
			else if (currentVelocityMult < -localShip.Ship.MoveSpeed)
			{
				currentVelocityMult = -localShip.Ship.MoveSpeed;
			}
		}
	}
}
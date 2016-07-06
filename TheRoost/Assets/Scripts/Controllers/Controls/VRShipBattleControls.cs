using UnityEngine;
using System.Collections;
using Models;
using Services;
using Utils;
using Events;

namespace Controllers.Controls
{
	public class VRShipBattleControls
	{
		private readonly Vector3 SHIP_GRAB_POS = new Vector3 (0f, -0.125f, 0.235f);
		private readonly Vector3 SHIP_DEBUG_POS = new Vector3 (0f, 0f, 1.25f);
		private readonly Quaternion SHIP_GRAB_ROT = 
			new Quaternion (0.004537996f, -0.9590314f, -0.2831307f, 0.008671289f);

		private VRInteractionControls controls;
		private EntityController entityController;
		private GameObject leftController;
		private GameObject rightController;
		private GameObject shipController;
		private GameObject aimingController;
		private GameObject vrRig;
		private ShipEntity localShip;
		private TargetingEntity targetingEntity;
		private GameObject spaceDust;
		private bool shipIsGrabbed;

		private float lastScrollYVal;
		private float currentVelocityMult;

		public VRShipBattleControls(ShipEntity localShip, EntityController entityController)
		{
			controls = new VRInteractionControls ();
			this.localShip = localShip;
			this.entityController = entityController;
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

					Ray camRay = debugCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
					Vector3 lookPt = debugCamera.transform.position + (camRay.direction * 40f);
					targetingEntity.Model.transform.LookAt(lookPt);
				}
				Service.Network.BroadcastCurrentTransform (localShip);

				if (targetingEntity != null)
				{
					Service.Network.BroadcastCurrentTransform (targetingEntity);
				}
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

				aimingController = lDist < rDist ? rightController : leftController;
				targetingEntity = entityController.AddLocalTargetingEntity (aimingController);
				targetingEntity.Model.transform.SetParent(aimingController.transform);
				targetingEntity.Model.transform.localPosition = Vector3.zero;
				targetingEntity.Model.transform.localEulerAngles = Vector3.zero;

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

					targetingEntity = entityController.AddLocalTargetingEntity (debugCamera);
					targetingEntity.Model.transform.SetParent(debugCamera.transform);
					targetingEntity.Model.transform.localPosition = SHIP_DEBUG_POS;
					targetingEntity.Model.transform.localEulerAngles = Vector3.zero;
				}
			}
			controls.UnregisterOnPress(localShip.Model, OnGrabShip);
			Service.Events.AddListener(EventId.VRControllerTriggerPress, OnFire);
			Service.Events.AddListener(EventId.VRControllerTouchpadPress, OnSwitchWeapons);
		}

		private void OnFire(object cookie)
		{
			VRTriggerInteraction interaction = (VRTriggerInteraction)cookie;
			if(interaction.ControllerObject == aimingController || vrRig == null || !vrRig.activeSelf)
			{
				localShip.Fire(targetingEntity.AimPosition, targetingEntity);
			}
		}

		private void OnSwitchWeapons(object cookie)
		{
			VRTouchpadInteraction interaction = (VRTouchpadInteraction)cookie;
			if(interaction.ControllerObject == aimingController || vrRig == null || !vrRig.activeSelf)
			{
				localShip.SwitchWeapons(1);
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
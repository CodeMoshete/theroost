using System;
using Controllers.Controls;
using Controllers.Interfaces;
using Events;
using HammerEditor.Main.Cameras.ControlSystems;
using Monobehaviors;
using Services;
using UnityEngine;

namespace Controllers
{
	public class DebugCameraController : IUpdateObserver
	{
		public const string DEBUG_CAMERA_NAME = "DebugCamera";

		private bool debugCameraOn;
		private Camera debugCamera;
		private GameObject cameraRig;
		private DebugCameraControls debugControls;
		private SteamVR_Controller.Device mouseDevice;
		private Transform currentHoverObject;

		public DebugCameraController ()
		{
			Service.FrameUpdate.RegisterForUpdate (this);
			cameraRig = GameObject.Find("[CameraRig]");
			mouseDevice = new SteamVR_Controller.Device (1337);
		}

		public void Update(float dt)
		{
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.D))
			{
				ToggleDebugCamera();
			}

			if(debugCameraOn)
			{
				debugControls.Update (dt);
				Ray mouseRay = debugCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				bool didRaycastHit = Physics.Raycast (mouseRay, out hit);
				if (didRaycastHit && hit.transform != currentHoverObject)
				{
					currentHoverObject = hit.transform;
					VRControllerInteraction interaction = 
						new VRControllerInteraction (mouseDevice, hit.transform.gameObject);
					Service.Events.SendEvent (EventId.VRHandCollisionEnter, interaction);
				}
				else if(!didRaycastHit && currentHoverObject != null)
				{
					VRControllerInteraction interaction = 
						new VRControllerInteraction (mouseDevice,currentHoverObject.gameObject);
					Service.Events.SendEvent (EventId.VRHandCollisionExit, interaction);
					currentHoverObject = null;
				}

				VRTriggerInteraction triggerInteraction;
				if (currentHoverObject != null && Input.GetMouseButtonDown (0))
				{
					triggerInteraction = new VRTriggerInteraction(mouseDevice, currentHoverObject.gameObject);
					Service.Events.SendEvent (EventId.VRControllerTriggerPress, triggerInteraction);
				}
				else if (currentHoverObject != null && Input.GetMouseButtonUp (0))
				{
					triggerInteraction = new VRTriggerInteraction(mouseDevice, currentHoverObject.gameObject);
					Service.Events.SendEvent (EventId.VRControllerTriggerRelease, triggerInteraction);
				}
				else if(currentHoverObject == null)
				{
					triggerInteraction = new VRTriggerInteraction(mouseDevice, null);
					Service.Events.SendEvent (EventId.VRControllerTriggerRelease, triggerInteraction);
				}
			}
		}

		private void ToggleDebugCamera()
		{
			debugCameraOn = !debugCameraOn;
			if (debugCameraOn)
			{
				GameObject camObject = new GameObject(DEBUG_CAMERA_NAME);
				debugCamera = camObject.AddComponent<Camera>();
				debugControls = new DebugCameraControls ();
				debugControls.Initialize (debugCamera);
				cameraRig.SetActive(false);
			}
			else
			{
				GameObject.Destroy(debugCamera.gameObject);
				debugCamera = null;
				cameraRig.SetActive(true);
			}
			Service.Events.SendEvent (EventId.DebugCameraControlsActive, debugCameraOn);
		}
	}
}


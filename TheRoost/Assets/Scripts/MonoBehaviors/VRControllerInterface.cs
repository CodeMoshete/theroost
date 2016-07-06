using UnityEngine;
using System.Collections;
using Services;
using Events;
using Game.Enums;

namespace Monobehaviors
{
	public class VRControllerInterface : MonoBehaviour 
	{
		private SteamVR_TrackedObject controller;
		private bool isTouchDown;

		public void Start()
		{
			controller = gameObject.GetComponent<SteamVR_TrackedObject> ();
		}

		public void Update()
		{
			SteamVR_Controller.Device ctrl = SteamVR_Controller.Input ((int)controller.index);

			// Handle trigger input
			if (ctrl.GetPressDown (SteamVR_Controller.ButtonMask.Trigger))
			{
				VRTriggerInteraction interaction = new VRTriggerInteraction(ctrl, gameObject);
				Service.Events.SendEvent (EventId.VRControllerTriggerPress, interaction);
			}

			if (ctrl.GetPressUp (SteamVR_Controller.ButtonMask.Trigger))
			{
				VRTriggerInteraction interaction = new VRTriggerInteraction(ctrl, gameObject);
				Service.Events.SendEvent (EventId.VRControllerTriggerRelease, interaction);
			}

			// Handle touchpad input
			VRTouchpadInteraction touch;
			if (ctrl.GetTouch (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
			{
				if (!isTouchDown)
				{
					touch = new VRTouchpadInteraction (gameObject, ctrl.GetAxis ());
					Service.Events.SendEvent(EventId.VRControllerTouchpadPress, touch);
					isTouchDown = true;
				}
				else
				{
					touch = new VRTouchpadInteraction (gameObject, ctrl.GetAxis ());
					Service.Events.SendEvent(EventId.VRControllerTouchpadDrag, touch);
				}
			}
			else if(isTouchDown)
			{
				touch = new VRTouchpadInteraction (gameObject, ctrl.GetAxis ());
				Service.Events.SendEvent(EventId.VRControllerTouchpadRelease, touch);
				isTouchDown = false;
			}
		}

		public void OnTriggerEnter(Collider other)
		{
			SteamVR_Controller.Device ctrl = SteamVR_Controller.Input ((int)controller.index);
			VRControllerInteraction interaction = new VRControllerInteraction (ctrl, other.gameObject);
			Service.Events.SendEvent (EventId.VRHandCollisionEnter, interaction);
		}

		public void OnTriggerExit(Collider other)
		{
			SteamVR_Controller.Device ctrl = SteamVR_Controller.Input ((int)controller.index);
			VRControllerInteraction interaction = new VRControllerInteraction (ctrl, other.gameObject);
			Service.Events.SendEvent (EventId.VRHandCollisionExit, interaction);
		}
	}
}
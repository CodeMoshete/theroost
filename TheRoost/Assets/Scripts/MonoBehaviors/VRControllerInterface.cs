using UnityEngine;
using System.Collections;
using Services;
using Events;

namespace Monobehaviors
{
	public class VRControllerInterface : MonoBehaviour 
	{
		private SteamVR_TrackedObject controller;

		public void Start()
		{
			controller = gameObject.GetComponent<SteamVR_TrackedObject> ();
		}

		public void Update()
		{
			SteamVR_Controller.Device ctrl = SteamVR_Controller.Input ((int)controller.index);

			if (ctrl.GetPressDown (SteamVR_Controller.ButtonMask.Trigger))
				Service.Events.SendEvent (EventId.VRControllerTriggerPress, ctrl);

			if (ctrl.GetPressUp (SteamVR_Controller.ButtonMask.Trigger))
				Service.Events.SendEvent (EventId.VRControllerTriggerRelease, ctrl);
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
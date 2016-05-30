using UnityEngine;
using System.Collections;
using Services;
using Events;
using System.Collections.Generic;
using Controllers.Interfaces;
using System;

namespace Controllers.Controls
{
	public class VRInteractionControls
	{
		private const ushort BUTTON_INTERACT_PULSE = 1500;

		private Dictionary<SteamVR_Controller.Device, GameObject> hoverButtons;
		private Dictionary<GameObject, List<Action>> pressInteractions;
		private Dictionary<GameObject, List<Action>> enterInteractions;
		private Dictionary<GameObject, List<Action>> exitInteractions;

		public VRInteractionControls()
		{
			pressInteractions = new Dictionary<GameObject, List<Action>> ();
			enterInteractions = new Dictionary<GameObject, List<Action>> ();
			exitInteractions = new Dictionary<GameObject, List<Action>> ();
			hoverButtons = new Dictionary<SteamVR_Controller.Device, GameObject> ();
			Service.Events.AddListener (EventId.VRHandCollisionEnter, OnHandEnter);
			Service.Events.AddListener (EventId.VRHandCollisionExit, OnHandExit);
			Service.Events.AddListener (EventId.VRControllerTriggerPress, OnTriggerPress);
			Service.Events.AddListener (EventId.VRControllerTriggerPress, OnTriggerRelease);
		}

		public void RegisterOnPress(GameObject target, Action callback)
		{
			if (!pressInteractions.ContainsKey (target))
			{
				pressInteractions.Add (target, new List<Action> ());
			}
			pressInteractions [target].Add (callback);
		}

		public void UnregisterOnPress(GameObject target, Action callback)
		{
			if (pressInteractions.ContainsKey (target) && pressInteractions[target].Contains(callback))
			{
				pressInteractions[target].Remove (callback);
			}
		}

		public void RegisterOnEnter(GameObject target, Action callback)
		{
			if (!enterInteractions.ContainsKey (target))
			{
				enterInteractions.Add (target, new List<Action> ());
			}
			enterInteractions [target].Add (callback);
		}

		public void UnregisterOnEnter(GameObject target, Action callback)
		{
			if (enterInteractions.ContainsKey (target) && enterInteractions[target].Contains(callback))
			{
				enterInteractions[target].Remove (callback);
			}
		}

		public void RegisterOnExit(GameObject target, Action callback)
		{
			if (!exitInteractions.ContainsKey (target))
			{
				exitInteractions.Add (target, new List<Action> ());
			}
			exitInteractions [target].Add (callback);
		}

		public void UnregisterOnExit(GameObject target, Action callback)
		{
			if (exitInteractions.ContainsKey (target) && exitInteractions[target].Contains(callback))
			{
				exitInteractions[target].Remove (callback);
			}
		}

		private void OnHandEnter(object cookie)
		{
			VRControllerInteraction interaction = (VRControllerInteraction)cookie;
			SetPotentialInteraction (interaction.Controller, interaction.CollisionObject);
			interaction.Controller.TriggerHapticPulse (BUTTON_INTERACT_PULSE);

			if (hoverButtons.ContainsKey (interaction.Controller) && 
				hoverButtons [interaction.Controller] != null && 
				enterInteractions.ContainsKey(hoverButtons[interaction.Controller]))
			{
				for (int i = 0, ct = enterInteractions[hoverButtons [interaction.Controller]].Count; i < ct; i++)
				{
					enterInteractions [hoverButtons [interaction.Controller]] [i] ();
				}
			}
		}

		private void OnHandExit(object cookie)
		{
			VRControllerInteraction interaction = (VRControllerInteraction)cookie;

			if (hoverButtons.ContainsKey (interaction.Controller) && 
				hoverButtons [interaction.Controller] != null && 
				exitInteractions.ContainsKey(hoverButtons[interaction.Controller]))
			{
				for (int i = 0, ct = exitInteractions[hoverButtons [interaction.Controller]].Count; i < ct; i++)
				{
					exitInteractions [hoverButtons [interaction.Controller]] [i] ();
				}
			}

			if (hoverButtons.ContainsKey (interaction.Controller) && 
				hoverButtons [interaction.Controller] == interaction.CollisionObject)
			{
				SetPotentialInteraction (interaction.Controller, null);
			}

			interaction.Controller.TriggerHapticPulse (BUTTON_INTERACT_PULSE);
		}

		private void SetPotentialInteraction(SteamVR_Controller.Device device, GameObject interactible)
		{
			if (!hoverButtons.ContainsKey (device))
			{
				hoverButtons.Add (device, interactible);
			}
			else
			{
				hoverButtons [device] = interactible;
			}
		}

		private void OnTriggerPress(object cookie)
		{
			SteamVR_Controller.Device device = (SteamVR_Controller.Device)cookie;
			if (hoverButtons.ContainsKey (device) && 
				hoverButtons [device] != null && 
				pressInteractions.ContainsKey(hoverButtons[device]))
			{
				for (int i = 0, ct = pressInteractions[hoverButtons [device]].Count; i < ct; i++)
				{
					pressInteractions [hoverButtons [device]] [i] ();
				}
			}
		}

		private void OnTriggerRelease(object cookie)
		{
			// Do nothing for now...
		}

		public void Unload()
		{
			pressInteractions = null;
			hoverButtons = null;
			Service.Events.RemoveListener (EventId.VRHandCollisionEnter, OnHandEnter);
			Service.Events.RemoveListener (EventId.VRHandCollisionExit, OnHandExit);
			Service.Events.RemoveListener (EventId.VRControllerTriggerPress, OnTriggerPress);
			Service.Events.RemoveListener (EventId.VRControllerTriggerPress, OnTriggerRelease);
		}
	}
}
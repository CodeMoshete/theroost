using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using Controllers.Interfaces;

namespace Models
{
	public class AIShipEntity : ShipEntity, IUpdateObserver
	{
		private const float BEHAVIOR_COOLDOWN = 3f;

		public Entity CurrentTarget;
		private List<IComponent> components;
		private Dictionary<ComponentEventId, List<IComponent>> listeners;
		private float currentCooldown;

		public AIShipEntity(ShipEntry ship, string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base( ship, id, spawnPos, spawnRot)
		{
			listeners = new Dictionary<ComponentEventId, List<IComponent>> ();
			components = new List<IComponent> ();
			Service.FrameUpdate.RegisterForUpdate (this);
		}

		public void RegisterComponentListener(IComponent component, ComponentEventId evt)
		{
			if (!listeners.ContainsKey (evt))
			{
				listeners.Add (evt, new List<IComponent> ());
			}

			listeners [evt].Add (component);
		}

		public void RemoveComponent(IComponent component)
		{
			components.Remove (component);

			foreach (KeyValuePair<ComponentEventId, List<IComponent>> pair in listeners)
			{
				if (pair.Value.Contains (component))
				{
					pair.Value.Remove (component);
				}
			}
		}

		public void Update(float dt)
		{
			for (int i = 0, count = components.Count; i < count; ++i)
			{
				components[i].Update (dt);
			}
		}

		public void SendComponentEvent(ComponentEventId evt)
		{
			if(listeners.ContainsKey(evt))
			{
				List<IComponent> components = listeners [evt];
				for (int i = 0, count = components.Count; i < count; ++i)
				{
					components[i].OnComponentEvent (evt);
				}
			}
		}
	}
}
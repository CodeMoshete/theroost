using UnityEngine;
using System.Collections;
using Events;
using System.Collections.Generic;

public class EventService  
{
	public delegate void Listener(object cookie);
	private Dictionary<EventId, List<Listener>> listeners;

	public EventService()
	{
		listeners = new Dictionary<EventId, List<Listener>> ();
	}

	public void AddListener(EventId type, Listener callback)
	{
		if (!listeners.ContainsKey (type))
		{
			listeners.Add (type, new List<Listener> ());
		}
			
		listeners[type].Add(callback);
	}

	public void RemoveListener(EventId type, Listener callback)
	{
		if (listeners.ContainsKey (type))
		{
			listeners[type].Remove(callback);
		}
	}

	public void SendEvent(EventId type, object cookie)
	{
		if (listeners.ContainsKey (type))
		{
			List<Listener> typeListeners = listeners [type];
			for (int i = 0, ct = typeListeners.Count; i < ct; i++)
			{
				typeListeners[i](cookie);
			}
		}
	}

	public void RemoveAllListeners()
	{
		listeners = new Dictionary<EventId, List<Listener>> ();
	}
}

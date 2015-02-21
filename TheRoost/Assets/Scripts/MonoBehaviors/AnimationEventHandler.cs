//AnimationEventHandler.cs
//Passes animation events back from a GameObject to an observer class.

using System;
using MonoBehaviors.Interfaces;
using System.Collections.Generic;

namespace MonoBehaviors
{
	public class AnimationEventHandler : UnityEngine.MonoBehaviour
	{
		private List<IAnimationEventObserver> m_observers;

		public AnimationEventHandler ()
		{
			m_observers = new List<IAnimationEventObserver>();
		}

		public void RegisterForAnimationEvents(IAnimationEventObserver observer)
		{
			m_observers.Add(observer);
		}
		
		public void UnregisterForAnimationEvents(IAnimationEventObserver observer)
		{
			m_observers.Remove(observer);
		}

		//Call this method from your animation
		public void OnAnimationEvent(string eventName)
		{
			for(int i = 0, count = m_observers.Count; i < count; i++)
			{
				m_observers[i].OnAnimationEvent(eventName);
			}
		}
	}
}


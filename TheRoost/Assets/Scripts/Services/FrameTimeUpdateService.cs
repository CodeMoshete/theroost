//FrameTimeUpdateController.cs
//Global handler for all update methods.

using System;
using Controllers.Interfaces;
using System.Collections.Generic;


namespace Services
{
	public class FrameTimeUpdateService
	{
		private List<IUpdateObserver> m_observers;

		public FrameTimeUpdateService ()
		{
			m_observers = new List<IUpdateObserver>();
		}

		public void RegisterForUpdate(IUpdateObserver observer)
		{
			m_observers.Add(observer);
		}

		public void UnregisterForUpdate(IUpdateObserver observer)
		{
			m_observers.Remove(observer);
		}

		//This method should ONLY be called by the Update() method in Engine.cs
		public void Update(Engine engineEnforcer)
		{
			float dt = UnityEngine.Time.deltaTime;
			for(int i = 0, count = m_observers.Count; i < count; i++)
			{
				m_observers[i].Update(dt);
			}
		}
	}
}


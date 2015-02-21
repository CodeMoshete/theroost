//FrameTimeUpdateController.cs
//Global handler for all update methods.

using System;
using Controllers.Interfaces;
using System.Collections.Generic;


namespace Controllers
{
	public class FrameTimeUpdateController
	{
		private static FrameTimeUpdateController m_instance;

		private List<IUpdateObserver> m_observers;

		public FrameTimeUpdateController (object enforcer)
		{
			if(!(enforcer is SingletonEnforcer) || enforcer == null)
			{
				UnityEngine.Debug.LogError("ERROR - FrameTimeUpdateController must be accessed via the GetInstance() method!");
			}

			m_observers = new List<IUpdateObserver>();
		}

		public static FrameTimeUpdateController GetInstance()
		{
			if(m_instance == null)
			{
				m_instance = new FrameTimeUpdateController(new SingletonEnforcer());
			}

			return m_instance;
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

	internal class SingletonEnforcer
	{
		public SingletonEnforcer()
		{
		}
	}
}


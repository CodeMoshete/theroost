//IAnimationEventObserver.cs
//Interface for classes that listen for animation events.

using System;

namespace MonoBehaviors.Interfaces
{
	public interface IAnimationEventObserver
	{
		void OnAnimationEvent(string eventType);
	}
}


//IUpdateObserver.cs
//Interface for classes that require an update method.

using System;

namespace Controllers.Interfaces
{
	public interface IUpdateObserver
	{
		void Update(float dt);
	}
}


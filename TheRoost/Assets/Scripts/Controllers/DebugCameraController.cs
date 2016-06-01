using System;
using Services;
using Controllers.Interfaces;
using UnityEngine;

namespace Controllers
{
	public class DebugCameraController : IUpdateObserver
	{
		private bool debugCameraOn;
		public Camera DebugCamera { get; private set; }

		public DebugCameraController ()
		{
			Service.FrameUpdate.RegisterForUpdate (this);
		}

		public void Update(float dt)
		{
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.D))
			{

			}
		}

		private void ToggleDebugCamera()
		{
			debugCameraOn = !debugCameraOn;
			if (debugCameraOn)
			{
				GameObject
			}
		}
	}
}


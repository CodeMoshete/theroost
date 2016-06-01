using System;
using Services;
using Controllers.Interfaces;
using UnityEngine;

namespace Controllers
{
	public class DebugCameraController : IUpdateObserver
	{
		private bool debugCameraOn;
		private Camera debugCamera;
		private GameObject cameraRig;
		private 

		public DebugCameraController ()
		{
			Service.FrameUpdate.RegisterForUpdate (this);
			cameraRig = GameObject.Find("CameraRig");
		}

		public void Update(float dt)
		{
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.D))
			{
				ToggleDebugCamera();
			}

			if(debugCameraOn)
			{
				Ray mouseRay = debugCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(mouseRay, out hit))
				{

				}
			}
		}

		private void ToggleDebugCamera()
		{
			debugCameraOn = !debugCameraOn;
			if (debugCameraOn)
			{
				GameObject camObject = new GameObject();
				debugCamera = camObject.AddComponent<Camera>();
				cameraRig.SetActive(false);
			}
			else
			{
				GameObject.Destroy(debugCamera.gameObject);
				debugCamera = null;
				cameraRig.SetActive(true);
			}
		}
	}
}


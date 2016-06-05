//BattleController.cs
//Controller class for the battle.

using System;
using Controllers.Controls;
using Controllers.Interfaces;
using Game.Controllers.Interfaces;
using Models;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Controllers.Types;

namespace Controllers
{
	public class BattleController : IStateController, IUpdateObserver
	{
		private EntityController entityController;
		private VRInteractionControls controls;
		private SceneLoadedCallback onLoaded;
		private ShipEntity localShip;
		private ShipEntry selectedShip;

		private GameObject vrRig;
		private GameObject leftController;
		private GameObject rightController;

		private bool shipIsGrabbed;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
			onLoaded = onLoadedCallback;
			BattleLoadParams loadParams = (BattleLoadParams)passedParams;
			selectedShip = loadParams.Ship;
			controls = new VRInteractionControls ();
			entityController = new EntityController ();
			vrRig = GameObject.Find ("[CameraRig]");
			leftController = GameObject.Find ("Controller (left)");
			rightController = GameObject.Find ("Controller (right)");
			Service.Network.Connect (OnConnectionMade);
		}

		private void OnConnectionMade()
		{
			onLoaded ();
		}

		public void Start()
		{
			Service.FrameUpdate.RegisterForUpdate(this);
			// TODO: Base this off of player ID rather than IsMaster so we can support more than 2 player positions.
			Vector3 spawnPos = Service.Network.IsMaster ? new Vector3(0f, 1f, -1.5f) : new Vector3(0f, 1f, 1.5f);
			localShip = entityController.AddLocalShip (selectedShip, spawnPos);
			controls.RegisterOnPress (localShip.Model, OnGrabShip);
		}

		public void OnGrabShip()
		{
			if (vrRig != null && vrRig.activeSelf)
			{
				float lDist = 
					Vector3.SqrMagnitude (leftController.transform.position - localShip.Model.transform.position);
				float rDist = 
					Vector3.SqrMagnitude (rightController.transform.position - localShip.Model.transform.position);

				Transform newParent = lDist < rDist ? leftController.transform : rightController.transform;
				localShip.Model.transform.SetParent (newParent);
				shipIsGrabbed = true;
			}
			else
			{
				GameObject debugCamera = GameObject.Find ("DebugCamera");
				if (debugCamera != null)
				{
					localShip.Model.transform.SetParent (debugCamera.transform);
					Vector3 startPos = new Vector3 (0f, -0.1f, 1f);
					Vector3 startEuler = new Vector3 (0f, 180f, 0f);
					localShip.Model.transform.localPosition = startPos;
					localShip.Model.transform.localEulerAngles = startEuler;
					shipIsGrabbed = true;
				}
			}
		}

		public void Update(float dt)
		{
			//Put update code in here.
			if (shipIsGrabbed)
			{
				Service.Network.BroadcastCurrentTransform (localShip);
			}
		}

		public void Unload()
		{
			Service.FrameUpdate.UnregisterForUpdate(this);
		}
	}
}


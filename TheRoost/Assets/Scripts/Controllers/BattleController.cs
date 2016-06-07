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
		private VRShipBattleControls battleControls;
		private SceneLoadedCallback onLoaded;
		private ShipEntity localShip;
		private ShipEntry selectedShip;
		private GameObject vrRig;
		private SteamVR_Controller.Device shipDevice;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
			onLoaded = onLoadedCallback;
			BattleLoadParams loadParams = (BattleLoadParams)passedParams;
			selectedShip = loadParams.Ship;
			entityController = new EntityController ();
			vrRig = GameObject.Find ("[CameraRig]");
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
			Vector3 spawnPos = Service.Network.IsMaster ? new Vector3(0f, 1f, 1.5f) : new Vector3(0f, 1f, -1.5f);
			localShip = entityController.AddLocalShip (selectedShip, spawnPos);
			battleControls = new VRShipBattleControls (localShip);
		}

		public void Update(float dt)
		{
			//Put update code in here.
			if (battleControls != null)
			{
				battleControls.Update ();
			}
		}

		public void Unload()
		{
			battleControls.Unload ();
			Service.FrameUpdate.UnregisterForUpdate(this);
		}
	}
}


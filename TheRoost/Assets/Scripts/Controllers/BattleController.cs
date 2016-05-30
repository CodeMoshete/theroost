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
		private EntityLoadController entityController;
		private VRInteractionControls controls;
		private SceneLoadedCallback onLoaded;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
			onLoaded = onLoadedCallback;
			BattleLoadParams loadParams = (BattleLoadParams)passedParams;
			controls = new VRInteractionControls ();
			entityController = new EntityLoadController ();
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
			Vector3 spawnPos = Service.Network.IsMaster ? new Vector3(0f, 0f, -10f) : new Vector3(0f, 0f, 10f);
			entityController.AddShip (ShipEntry.GalaxyClass, spawnPos);
		}

		public void Update(float dt)
		{
			//Put update code in here.
		}

		public void Unload()
		{
			Service.FrameUpdate.UnregisterForUpdate(this);
		}
	}
}


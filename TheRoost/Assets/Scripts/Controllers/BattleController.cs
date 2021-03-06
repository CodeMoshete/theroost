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
using Monobehaviors;

namespace Controllers
{
	public class BattleController : IStateController, IUpdateObserver
	{
		private EntityController entityController;
		private VRShipBattleControls battleControls;
		private SceneLoadedCallback onLoaded;
		private ShipEntity localShip;
		private ShipEntry selectedShip;
		private MapEntry selectedMap;
		private MapEntity localMap;
		private GameObject vrRig;
		private SteamVR_Controller.Device shipDevice;

		private bool isMaster;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
			onLoaded = onLoadedCallback;
			BattleLoadParams loadParams = (BattleLoadParams)passedParams;
			selectedShip = loadParams.Ship;
			selectedMap = loadParams.Map;
			entityController = new EntityController ();
			vrRig = GameObject.Instantiate(Resources.Load<GameObject>("[CameraRig]"));
			AddControllerComponents ("Controller (left)");
			AddControllerComponents ("Controller (right)");
			Service.Network.Connect (OnConnectionMade);
		}

		private void AddControllerComponents(string controllerName)
		{
			GameObject controller = UnityUtils.FindGameObject (vrRig, controllerName);
			if (controller.GetComponent<SphereCollider> () == null)
			{
				controller.AddComponent<SphereCollider> ();
			}

			if (controller.GetComponent<VRControllerInterface> () == null)
			{
				controller.AddComponent<VRControllerInterface> ();
			}
		}

		private void OnConnectionMade()
		{
			onLoaded ();
		}

		public void Start()
		{
			isMaster = Service.Network.IsMaster;
			Service.FrameUpdate.RegisterForUpdate(this);
			// TODO: Base this off of player ID rather than IsMaster so we can support more than 2 player positions.
			Vector3 spawnPos = isMaster ? new Vector3(0f, 1f, 1.5f) : new Vector3(0f, 1f, -1.5f);
			localShip = entityController.AddLocalShip (selectedShip, spawnPos);
			battleControls = new VRShipBattleControls (localShip, entityController);

			if (isMaster)
			{
				localMap = entityController.AddLocalMap (selectedMap);
			}
		}

		public void Update(float dt)
		{
			//Put update code in here.
			if (battleControls != null)
			{
				battleControls.Update ();
			}

			entityController.Update(dt);
		}

		public void Unload()
		{
			battleControls.Unload ();
			Service.FrameUpdate.UnregisterForUpdate(this);
			localShip.Unload ();
			localShip = null;
			localMap.Unload ();
			localMap = null;
		}
	}
}

//MainMenuPedestalController.cs
//Controller class for the main menu. Essentially a game state.

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
	public class MainMenuPedestalController : IStateController, IUpdateObserver
	{
		private const string PEDESTAL_ID = "Models/Pedestal";
		private readonly ShipEntry[] SHIP_ENTRIES = { ShipEntry.GalaxyClass, ShipEntry.Gunship, ShipEntry.Skirmisher, ShipEntry.Interceptor};

		private GameObject pedestal;
		private GameObject startButton;
		private VRInteractionControls controls;
		private Action<ShipEntry> onStartBattle;
		private GameObject leftButton;
		private GameObject rightButton;
		private GameObject shipCenter;
		private Entity currentShipModel;
		private ShipEntry currentShip; 
		private int shipIndex;


		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
			MainMenuLoadParams loadParams = (MainMenuLoadParams)passedParams;
			onStartBattle = loadParams.OnBattleStart;
			pedestal = GameObject.Instantiate(Resources.Load<GameObject>(PEDESTAL_ID)) as GameObject;
			onLoadedCallback();
		}

		public void Start()
		{
			Service.FrameUpdate.RegisterForUpdate (this);
			controls = new VRInteractionControls ();
			startButton = UnityUtils.FindGameObject (pedestal, "SwitchButton");
			controls.RegisterOnPress (startButton, OnStartButtonPressed);
			controls.RegisterOnEnter (startButton, OnStartButtonOver);
			controls.RegisterOnExit (startButton, OnStartButtonOut);
			leftButton = UnityUtils.FindGameObject (pedestal, "arrow left");
			controls.RegisterOnPress (leftButton, OnleftButtonPressed);
			rightButton = UnityUtils.FindGameObject (pedestal, "arrow right");
			controls.RegisterOnPress (rightButton, OnrightButtonPressed);
			shipCenter = UnityUtils.FindGameObject (pedestal, "ShipCenter");
			currentShip = SHIP_ENTRIES [shipIndex];
			loadShip (shipIndex);
		}
				

		private void OnStartButtonPressed()
		{
			// TODO: Pass through a dynamic value determined by UI.
			onStartBattle (currentShip);
		}

		private void OnleftButtonPressed()
		{
			if (shipIndex >= SHIP_ENTRIES.Length - 1) {
				shipIndex = 0;
			} else {
				shipIndex++;
			}

			currentShip = SHIP_ENTRIES [shipIndex];
			loadShip (shipIndex);

		}

		private void OnrightButtonPressed()
		{	
			if (shipIndex <= 0 ) {
				shipIndex = SHIP_ENTRIES.Length - 1;
			} else {
				shipIndex--;
			}

			currentShip = SHIP_ENTRIES [shipIndex];
			loadShip (shipIndex);

		}

		private void loadShip(int Index)
		{	
			if (currentShipModel != null)
			{
				currentShipModel.Unload ();
			}
		
			currentShipModel = new ShipEntity (currentShip, "1", shipCenter.transform.position, Vector3.zero);
			currentShipModel.Model.GetComponent < Collider > ().enabled = false;
		}

		private void OnStartButtonOver()
		{
			startButton.GetComponent<Animator>().SetBool("hover", true);
		}

		private void OnStartButtonOut()
		{
			startButton.GetComponent<Animator>().SetBool("hover", false);
		}

		public void Update(float dt)
		{
			//Put update code in here.
		}

		public void Unload()
		{
			currentShipModel.Unload ();
			currentShipModel = null;
			controls.UnregisterOnPress (startButton, OnStartButtonPressed);
			controls.UnregisterOnEnter (startButton, OnStartButtonOver);
			controls.UnregisterOnExit (startButton, OnStartButtonOut);
			controls.Unload ();
			startButton = null;
			Service.FrameUpdate.UnregisterForUpdate(this);
			GameObject.Destroy(pedestal);
			pedestal = null;
		}
	}
}


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

		private GameObject pedestal;
		private GameObject startButton;
		private VRInteractionControls controls;
		private Action<ShipEntry> onStartBattle;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
			MainMenuLoadParams loadParams = (MainMenuLoadParams)passedParams;
			onStartBattle = loadParams.OnBattleStart;
			pedestal = GameObject.Instantiate(Resources.Load<GameObject>(PEDESTAL_ID)) as GameObject;
			onLoadedCallback();
		}

		public void Start()
		{
			Service.FrameUpdate.RegisterForUpdate(this);
			controls = new VRInteractionControls ();
			startButton = UnityUtils.FindGameObject (pedestal, "SwitchButton");
			controls.RegisterOnPress (startButton, OnStartButtonPressed);
			controls.RegisterOnEnter (startButton, OnStartButtonOver);
			controls.RegisterOnExit (startButton, OnStartButtonOut);
		}

		private void OnStartButtonPressed()
		{
			// TODO: Pass through a dynamic value determined by UI.
			onStartBattle (ShipEntry.GalaxyClass);
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


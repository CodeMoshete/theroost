//FlowController
//Controller class that strings any and all game states together.

using System;
using System.Collections.Generic;
using Controllers.Types;
using Controllers;
using Game.Factories;

namespace Game.Controllers
{
	public class FlowController
	{
		private StateFactory sceneFactory;

		public FlowController()
		{
			sceneFactory = new StateFactory();
		}

		public void StartGame()
		{
			LoadMainMenu();
		}

		public void LoadMainMenu()
		{
			MainMenuLoadParams passedParams = new MainMenuLoadParams();
			sceneFactory.LoadScene<MainMenuPedestalController, MainMenuLoadParams>(OnSceneLoaded, passedParams);
		}

		public void OnSceneLoaded()
		{

		}
	}
}


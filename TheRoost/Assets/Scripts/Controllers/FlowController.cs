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
			MainMenuLoadParams passedParams = new MainMenuLoadParams(LoadBattle);
			sceneFactory.LoadScene<MainMenuPedestalController>(OnSceneLoaded, passedParams);
		}

		public void LoadBattle(ShipEntry chosenShip, MapEntry chosenMap)
		{
			BattleLoadParams passedParams = new BattleLoadParams(LoadMainMenu, chosenShip, chosenMap);
			sceneFactory.LoadScene<BattleController>(OnSceneLoaded, passedParams);
		}

		public void OnSceneLoaded()
		{
			// Intentionally empty for now...
		}
	}
}


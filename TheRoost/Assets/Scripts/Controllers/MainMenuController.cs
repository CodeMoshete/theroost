//MainMenuController.cs
//Controller class for the main menu. Essentially a game state.

using System;
using Game.Controllers.Interfaces;
using UnityEngine;

namespace Controllers
{
	public class MainMenuController : ISceneController
	{
		private const string MAIN_MENU_ID = "GUI/gui_mainMenu";

		private GameObject m_mainMenu;

		public MainMenuController ()
		{
		}

		public void Load<S>(SceneLoadedCallback onLoadedCallback, S passedParams)
		{
			m_mainMenu = GameObject.Instantiate(Resources.Load<GameObject>(MAIN_MENU_ID)) as GameObject;
			m_mainMenu.SetActive(false);
			onLoadedCallback();
		}

		public void Start()
		{
			m_mainMenu.SetActive(true);
		}

		public void Unload()
		{
			GameObject.Destroy(m_mainMenu);
			m_mainMenu = null;
		}
	}
}


//MainMenuController.cs
//Controller class for the main menu. Essentially a game state.

using System;
using Game.Controllers.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
	public class MainMenuController : IStateController
	{
		private const string MAIN_MENU_ID = "GUI/gui_mainMenu";
		private const string CUBE_ID = "Models/Cube2";

		private GameObject m_mainMenu;
		private GameObject m_cube;

		public MainMenuController ()
		{
		}

		public void Load<S>(SceneLoadedCallback onLoadedCallback, S passedParams)
		{
			m_mainMenu = GameObject.Instantiate(Resources.Load<GameObject>(MAIN_MENU_ID)) as GameObject;
			m_mainMenu.SetActive(false);

			m_cube = GameObject.Instantiate(Resources.Load<GameObject>(CUBE_ID)) as GameObject;
			m_cube.SetActive(false);

			onLoadedCallback();
		}

		public void Start()
		{
			m_mainMenu.SetActive(true);
			m_cube.SetActive(true);
			m_cube.name = "Poop";
			GameObject.Find("Main Camera").transform.SetParent(m_cube.transform);
			GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(StartClicked);
		}

		private void StartClicked()
		{
			GameObject.Destroy(m_mainMenu);
		}

		public void Unload()
		{
			GameObject.Destroy(m_mainMenu);
			m_mainMenu = null;
		}
	}
}


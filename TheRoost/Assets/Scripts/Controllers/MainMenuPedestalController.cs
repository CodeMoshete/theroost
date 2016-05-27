//MainMenuPedestalController.cs
//Controller class for the main menu. Essentially a game state.

using System;
using Game.Controllers.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Controllers.Interfaces;

namespace Controllers
{
	public class MainMenuPedestalController : IStateController, IUpdateObserver
	{
		private const string PEDESTAL_ID = "Models/Pedestal";

		private GameObject pedestal;

		public MainMenuPedestalController ()
		{
		}

		public void Load<S>(SceneLoadedCallback onLoadedCallback, S passedParams)
		{
			pedestal = GameObject.Instantiate(Resources.Load<GameObject>(PEDESTAL_ID)) as GameObject;
			onLoadedCallback();
		}

		public void Start()
		{
			FrameTimeUpdateController.GetInstance().RegisterForUpdate(this);
		}

		private void StartClicked()
		{
		}

		public void Update(float dt)
		{
			//Put update code in here.
		}

		public void Unload()
		{
			FrameTimeUpdateController.GetInstance().UnregisterForUpdate(this);
			GameObject.Destroy(pedestal);
			pedestal = null;
		}
	}
}


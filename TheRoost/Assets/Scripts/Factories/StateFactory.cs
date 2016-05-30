// StateFactory.cs
// Controller class for Loading and Unloading game scenes

using System;
using System.Collections.Generic;
using GameObjectWrappers;
using UnityEngine;
using Game.Controllers.Interfaces;

namespace Game.Factories
{
	public class StateFactory
	{
//		private const string TRANSITION_SCREEN_ID = "GUI/gui_transition";
		private const string TRANSITION_SCREEN_ID = "GUI/TransitionScreen";

		private IStateController newScene;
		private IStateController currentScene;

		private SceneLoadedCallback onNewSceneLoaded;

		private bool isTransitionDone;
		private bool isSceneLoaded;

		private TransitionWrapper m_transitionScreen;

		public StateFactory()
		{
			isTransitionDone = true;//Don't show transition screen if it's our first load.
			isSceneLoaded = false;

			GameObject transitionScreen = 
				GameObject.Instantiate(Resources.Load<GameObject>(TRANSITION_SCREEN_ID)) as GameObject;
			m_transitionScreen = new TransitionWrapper(transitionScreen);
		}

		public void LoadScene<T>(SceneLoadedCallback callback, object passedParams) where T : IStateController, new()
		{
			onNewSceneLoaded = callback;

			//Don't show transition screen if it's our first load.
			if(!isTransitionDone)
			{
				m_transitionScreen.SetActive(true);
				m_transitionScreen.PlayTransitionOut(onTransitionShown);
			}

			newScene = new T();
			newScene.Load(onSceneLoaded, passedParams);
		}

		private void onTransitionShown()
		{
			isTransitionDone = true;
			if(isSceneLoaded)
			{
				onSceneReady();
			}
		}

		private void OnTransitionHidden()
		{
			m_transitionScreen.SetActive(false);
		}

		private void onSceneLoaded()
		{
			isSceneLoaded = true;
			if(isTransitionDone)
			{
				onSceneReady();
			}
		}

		public void onSceneReady()
		{
			isTransitionDone = isSceneLoaded = false;

			if(currentScene != null)
			{
				currentScene.Unload();
			}

			currentScene = newScene;
			currentScene.Start();
			m_transitionScreen.PlayTransitionIn(OnTransitionHidden);

			if(onNewSceneLoaded != null)
			{
				onNewSceneLoaded();
			}
		}

		public void StartScene<T>() where T : IStateController
		{
			if(currentScene is T)
			{
				currentScene.Start();
			}
		}
	}
}


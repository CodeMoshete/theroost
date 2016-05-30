//IStateController.cs

using System;
using System.Collections.Generic;

namespace Game.Controllers.Interfaces
{
	public delegate void SceneLoadedCallback();

	public interface IStateController
	{
		/// <summary>
		/// Method called from SceneFactory to commence loading for any assets this scene controller needs. IMPORTANT: Assets should NOT be added to the scene
		/// when they are done loading here. NO display logic should run until the SceneFactory calls the Start() method. Otherwise, scene content could display
		/// before the transition screen has fully covered the previous scene.
		/// </summary>
		/// <param name="onLoadedCallback">Callback to be called when the load sequence is complete.</param>
		/// <param name="passedParams">Any initialization parameters you need to pass into the new scene controller should be sent through here.</param>
		/// <typeparam name="S">The LoadParams Type for this scene controller.</typeparam>
		void Load(SceneLoadedCallback onLoadedCallback, object passedParams);

		/// <summary>
		/// Method called by the SceneFactory when the transition screen has covered the entire screen and the previous scene controller has finished unloading.
		/// Display logic should begin running here.
		/// </summary>
		void Start();

		/// <summary>
		/// Method called by the SceneFactory when a new scene controller has been started, and the transition screen has covered the entire screen. You should NOT
		/// remove any scene elements from the screen UNTIL this method has been called, otherwise you will end up with objects abruptly popping out of existence
		/// before the transition screen has finished covering the screen on the "out" transition.
		/// </summary>
		void Unload();
	}
}


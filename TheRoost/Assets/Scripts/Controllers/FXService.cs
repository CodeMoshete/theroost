using System;
using UnityEngine;
using Game.MonoBehaviors;
using System.Collections.Generic;

namespace Game.Controllers
{
	public class FXService
	{
		public Dictionary<string, string> ExplosionDebrisMap { get; private set; }

		public FXService()
		{
			ExplosionDebrisMap = new Dictionary<string, string> ();
			ExplosionDebrisMap.Add ("Metal", "FX/ExplosionDebris");
		}

		public void SpawnFX(string resourceName)
		{
			SpawnFX (resourceName, Vector3.zero);
		}

		public void SpawnFX(string resourceName, Vector3 position)
		{
			SpawnFX (resourceName, position, Vector3.zero);
		}

		public void SpawnFX(string resourceName, Vector3 position, Vector3 eulerAngles)
		{
			SpawnFX (resourceName, position, eulerAngles, Vector3.one);
		}

		public void SpawnFX(string resourceName, Vector3 position, Vector3 eulerAngles, Vector3 scale)
		{
			GameObject temp = GameObject.Instantiate (Resources.Load (resourceName) as GameObject);
			temp.AddComponent<TimedDestroy> ();
			temp.transform.position = position;
			temp.transform.eulerAngles = eulerAngles;
		}
	}
}
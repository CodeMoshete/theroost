using System;
using UnityEngine;

namespace Game.MonoBehaviors
{
	public class TimedDestroy : MonoBehaviour
	{
		private float lifetime;
		private bool isInitialized;

		public void Initialize(float lifetime)
		{
			isInitialized = true;
			this.lifetime = lifetime;
		}

		public void Update()
		{
			if (isInitialized && lifetime > 0f)
			{
				lifetime -= Time.deltaTime;

				if (lifetime <= 0f)
				{
					GameObject.Destroy (gameObject);
					isInitialized = false;
				}
			}
		}
	}
}


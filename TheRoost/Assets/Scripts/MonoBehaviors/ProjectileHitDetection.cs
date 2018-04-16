using UnityEngine;
using System.Collections;
using System.Reflection;
using Utils;
using System;

namespace MonoBehaviors
{
	public class ProjectileHitDetection : MonoBehaviour 
	{
		private Action<GameObject> onCollision;

		public void RegisterListener(Action<GameObject> onCollision)
		{
			this.onCollision = onCollision;
		}

		public void UnregisterListeners()
		{
			onCollision = null;
		}

		public void OnTriggerEnter(Collider other)
		{
			if(onCollision != null)
			{
				onCollision(other.gameObject);
			}
		}
	}
}
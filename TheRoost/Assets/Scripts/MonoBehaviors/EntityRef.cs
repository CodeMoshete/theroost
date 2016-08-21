using System;
using UnityEngine;
using Models;

namespace Game.MonoBehaviors
{
	public class EntityRef : MonoBehaviour
	{
		public ShipEntity Entity { get; private set; }

		public void Initialize(ShipEntity entity)
		{
			Entity = entity;
		}
	}
}


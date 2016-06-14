using UnityEngine;
using Game.Enums;
using System.Collections.Generic;
using Models.Interfaces;
using Utils;
using MonoBehaviors;

namespace Models
{
	public class TargetingEntity : Entity
	{
		private const string RESOURCE_NAME = "Models/TargetingRig";

		public TargetingEntity(string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base( id, RESOURCE_NAME, spawnPos, spawnRot, EntityType.TargetingRig, null)
		{
		}
	}
}
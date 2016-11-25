using UnityEngine;
using Game.Enums;
using System.Collections.Generic;
using Models.Interfaces;
using Utils;
using MonoBehaviors;
using Services;
using Events;
using Game.MonoBehaviors;

namespace Models
{
	public class MapEntity : Entity
	{
		public MapEntry Map { get; private set; }

		public MapEntity(MapEntry map, string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base( id, map.ResourceName, spawnPos, spawnRot, EntityType.Map, map.EntryName)
		{
			Map = map;

			Model.name = id;
			// EntityRef entityRef = Model.AddComponent<EntityRef> ();
			// entityRef.Initialize (this);
		}
	}
}
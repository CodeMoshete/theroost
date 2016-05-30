using System;
using Game.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers.Network.Types
{
	public class NetSpawnEntityType
	{
		/// Unique network-synced identifier for this entity.
		public string EntityId { get; private set; }
		public TeamID Team { get; private set; }
		public Vector3 SpawnPos { get; private set; }
		public Vector3 SpawnRot { get; private set; }

		/// Correlates to a static data entry class: i.e. ShipEntry.
		public EntityType EntityType { get; private set; }

		/// Correlates to an entry within a static data entry class: i.e. ShipEntry.GalaxyClass.
		public string EntryName { get; private set; }

		public NetSpawnEntityType (string entityId, 
								   TeamID team, 
								   Vector3 spawnPos, 
								   Vector3 lookAtPos,
								   string entityType,
								   string entryName)
		{
			EntityId = entityId;
			Team = team;
			SpawnPos = spawnPos;
			SpawnRot = lookAtPos;
			EntityType = (EntityType)Enum.Parse(typeof(EntityType), entityType);
			EntryName = entryName;
		}
	}
}


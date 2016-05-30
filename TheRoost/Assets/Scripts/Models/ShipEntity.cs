using UnityEngine;
using Game.Enums;

namespace Models
{
	public class ShipEntity : Entity
	{
		public ShipEntry Ship { get; private set; }

		public ShipEntity(ShipEntry ship, string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base( id, ship.ResourceName, spawnPos, spawnRot, EntityType.Ship, ship.EntryName)
		{
			Ship = ship;
		}
	}
}
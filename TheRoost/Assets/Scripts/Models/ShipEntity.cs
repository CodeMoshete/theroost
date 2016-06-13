using UnityEngine;
using Game.Enums;
using System.Collections.Generic;
using Models.Interfaces;
using Utils;
using MonoBehaviors;

namespace Models
{
	public class ShipEntity : Entity
	{
		public ShipEntry Ship { get; private set; }
		public Dictionary<string, Weapon> Turrets { get; private set; }

		public ShipEntity(ShipEntry ship, string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base( id, ship.ResourceName, spawnPos, spawnRot, EntityType.Ship, ship.EntryName)
		{
			Ship = ship;

			Turrets = new Dictionary<string, Weapon> ();
			List<GameObject> turrets = UnityUtils.FindAllGameObjectContains<WeaponPoint> (Model);
			for (int i = 0, ct = turrets.Count; i < ct; i++)
			{
				WeaponPoint turret = turrets [i].GetComponent<WeaponPoint> ();
				if (!Turrets.ContainsKey (turret.WeaponGroupId))
				{
					Turrets.Add(turret.WeaponGroupId, new Weapon(this, turret));
				}
			}
		}
	}
}
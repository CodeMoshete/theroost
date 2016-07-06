using UnityEngine;
using Game.Enums;
using System.Collections.Generic;
using Models.Interfaces;
using Utils;
using MonoBehaviors;
using Services;
using Events;

namespace Models
{
	public class ShipEntity : Entity
	{
		public ShipEntry Ship { get; private set; }
		public Dictionary<string, List<Weapon>> Turrets { get; private set; }
		private List<string> availableTurretTypes;
		private string selectedTurretType;

		public ShipEntity(ShipEntry ship, string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base( id, ship.ResourceName, spawnPos, spawnRot, EntityType.Ship, ship.EntryName)
		{
			Ship = ship;

			Model.name = id;

			availableTurretTypes = new List<string>();
			Turrets = new Dictionary<string, List<Weapon>> ();
			List<GameObject> turrets = UnityUtils.FindAllGameObjectContains<WeaponPoint> (Model);
			for (int i = 0, ct = turrets.Count; i < ct; i++)
			{
				WeaponPoint turret = turrets [i].GetComponent<WeaponPoint> ();
				if (!Turrets.ContainsKey (turret.WeaponGroupId))
				{
					Turrets.Add(turret.WeaponGroupId, new List<Weapon>());
					availableTurretTypes.Add(turret.WeaponGroupId);
				}

				if(string.IsNullOrEmpty(selectedTurretType))
				{
					selectedTurretType = turret.WeaponGroupId;
				}

				Turrets[turret.WeaponGroupId].Add(new Weapon(this, turret));
			}
		}

		public void Fire(Vector3 targetPos, TargetingEntity targetReticle)
		{
			List<Weapon> weapons = Turrets[selectedTurretType];
			for(int i = 0, count = weapons.Count; i < count; i++)
			{
				weapons[i].Fire(targetPos, targetReticle.Id);
			}
		}

		public void SwitchWeapons(int direction)
		{
			int currentIndex = availableTurretTypes.IndexOf(selectedTurretType);

			if(direction > 0)
			{
				if(currentIndex == (availableTurretTypes.Count - 1))
				{
					currentIndex = 0;
				}
				else
				{
					currentIndex++;
				}
			}
			else
			{
				if(currentIndex == 0)
				{
					currentIndex = availableTurretTypes.Count - 1;
				}
				else
				{
					currentIndex--;
				}
			}
			selectedTurretType = availableTurretTypes[currentIndex];
		}

		public WeaponPoint GetTurretById(string id)
		{
			foreach(KeyValuePair<string, List<Weapon>> pair in Turrets)
			{
				for(int i = 0, ct = pair.Value.Count; i < ct; i++)
				{
					WeaponPoint weaponPoint = pair.Value[i].WeaponPoint;
					if(string.Equals(weaponPoint.gameObject.name, id))
					{
						return weaponPoint;
					}
				}
			}

			return null;
		}
	}
}
using UnityEngine;
using Game.Enums;

namespace Models
{
	public class Entity 
	{
		/// Unique network-synced identifier for this entity.
		public string Id { get; private set; }

		/// This Entity's path in the Resources folder.h
		public string ResourceName { get; private set; }

		/// Correlates to a static data entry class: i.e. ShipEntry.
		public EntityType Type { get; private set; }

		/// Correlates to an entry within a static data entry class: i.e. ShipEntry.GalaxyClass.
		public string EntryName { get; private set; }

		/// This Entity's wrapped GameObject.
		public GameObject Model { get; private set; }

		/// Gets the spawn position.
		public int CurrentHealth { get; private set; }
		public int MaxHealth { get; private set; }
		public float HealthPct { get; private set; }

		public virtual Vector3 SpawnPos { get; private set; }
		public virtual Vector3 SpawnRotation { get; private set; }

		public Entity(string id, 
					  string resourceName, 
					  Vector3 spawnPos, 
					  Vector3 spawnRotation, 
					  EntityType entityType, 
					  string entryName)
		{
			Id = id;
			ResourceName = resourceName;
			SpawnPos = spawnPos;
			SpawnRotation = spawnRotation;
			Type = entityType;
			EntryName = entryName;

			Model = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(resourceName));
			Model.transform.position = spawnPos;
			Model.transform.eulerAngles = spawnRotation;
		}

		public void Unload ()
		{
			GameObject.Destroy (Model);
		}

		protected void SetBaseHealth(int baseHealth)
		{
			MaxHealth = baseHealth;
			CurrentHealth = baseHealth;
			HealthPct = 1f;
		}

		public void SetHealth(int value)
		{
			if (MaxHealth > 0)
			{
				CurrentHealth = value;
				HealthPct = (float)CurrentHealth / (float)MaxHealth;

				if (CurrentHealth <= 0)
				{
					PlayDeathAnimation ();
				}
			}
		}

		protected virtual void PlayDeathAnimation()
		{
			Model.SetActive (false);
		}
	}
}
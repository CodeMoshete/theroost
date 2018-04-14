using UnityEngine;
using Game.Enums;

namespace Models
{
	public class EnvironmentObjectEntity : Entity
	{
		public override Vector3 SpawnPos 
		{
			get
			{
				return Model.transform.position;
			}
		}

		public override Vector3 SpawnRotation 
		{
			get
			{
				return Model.transform.eulerAngles;
			}
		}

		public EnvironmentObjectEntity(EnvironmentObjectEntry entry, string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base(id, entry.ResourceName, spawnPos, spawnRot, EntityType.EnvironmentObject, entry.EntryName)
		{

		}
	}
}
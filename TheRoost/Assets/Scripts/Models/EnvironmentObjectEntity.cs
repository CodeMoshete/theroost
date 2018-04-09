using UnityEngine;
using Game.Enums;

namespace Models
{
	public class EnvironmentObjectEntity : Entity
	{
		public EnvironmentObjectEntity(EnvironmentObjectEntry entry, string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base(id, entry.ResourceName, spawnPos, spawnRot, EntityType.EnvironmentObject, entry.EntryName)
		{

		}
	}
}
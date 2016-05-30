using UnityEngine;
using System.Collections;
using Models;
using Services;
using System.Collections.Generic;
using Events;
using Game.Controllers.Network.Types;
using Game.Enums;

public class EntityLoadController
{
	private const string SHIP_PREFIX = "s*";
	private const string USER_PREFIX = "u*";

	private Dictionary<string, int> localEntityTypeCounts;

	public EntityLoadController()
	{
		localEntityTypeCounts = new Dictionary<string, int> ();
		Service.Events.AddListener (EventId.EntitySpawned, OnNetEntitySpawned);
	}

	public void AddEntity()
	{

	}

	public ShipEntity AddShip(ShipEntry ship, Vector3 spawnPos = new Vector3(), Vector3 spawnRot = new Vector3())
	{
		if (!localEntityTypeCounts.ContainsKey (ship.ResourceName))
		{
			localEntityTypeCounts.Add (ship.ResourceName, 0);
		}
		else
		{
			localEntityTypeCounts [ship.ResourceName]++;
		}

		string resCount = localEntityTypeCounts [ship.ResourceName].ToString ();
		string shipId = USER_PREFIX + Service.Network.PlayerId + SHIP_PREFIX + ship.ResourceName + resCount;

		return AddShipInternal (ship, spawnPos, spawnRot, shipId);
	}

	private ShipEntity AddShipInternal(ShipEntry ship, Vector3 spawnPos, Vector3 spawnRot, string uid)
	{
		ShipEntity entity = new ShipEntity (ship, uid, spawnPos, spawnRot);
		Service.Network.BroadcastEntitySpawned (entity);
		return entity;
	}

	private void OnNetEntitySpawned(object cookie)
	{
		NetSpawnEntityType spawnInfo = (NetSpawnEntityType)cookie;
		switch (spawnInfo.EntityType)
		{
			case EntityType.Ship:
				ShipEntry entry = typeof(ShipEntry).GetProperty (spawnInfo.EntryName).GetValue(null, null) as ShipEntry;
				AddShipInternal (entry, spawnInfo.SpawnPos, spawnInfo.SpawnRot, spawnInfo.EntityId);
				break;
		}
	}
}

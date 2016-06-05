using UnityEngine;
using System.Collections;
using Models;
using Services;
using System.Collections.Generic;
using Events;
using Game.Controllers.Network.Types;
using Game.Enums;

public class EntityController
{
	private const string SHIP_PREFIX = "s*";
	private const string USER_PREFIX = "u*";

	private Dictionary<string, int> localEntityTypeCounts;
	private Dictionary<string, Entity> trackedEntities;

	private ShipEntity localShip;

	public EntityController()
	{
		localEntityTypeCounts = new Dictionary<string, int> ();
		trackedEntities = new Dictionary<string, Entity> ();
		Service.Events.AddListener (EventId.NetPlayerConnected, OnPlayerConnected);
		Service.Events.AddListener (EventId.NetPlayerIdentified, OnPlayerIdentified);
		Service.Events.AddListener (EventId.NetPlayerDisconnected, OnPlayerDisconnected);
		Service.Events.AddListener (EventId.EntitySpawned, OnNetEntitySpawned);
		Service.Events.AddListener (EventId.EntityTransformUpdated, OnNetTransformUpdated);
	}

	private void OnPlayerConnected(object cookie)
	{
		NetSpawnEntityType spawnInfo = (NetSpawnEntityType)cookie;
		switch (spawnInfo.EntityType)
		{
			case EntityType.Ship:
				ShipEntry entry = typeof(ShipEntry).GetProperty (spawnInfo.EntryName).GetValue (null, null) as ShipEntry;
				Debug.Log ("Player connected: " + spawnInfo.EntityId);
				AddShipInternal (entry, spawnInfo.SpawnPos, spawnInfo.SpawnRot, spawnInfo.EntityId);
				Service.Network.BroadcastIdentification (localShip);
				break;
		}
	}

	private void OnPlayerIdentified(object cookie)
	{
		NetSpawnEntityType spawnInfo = (NetSpawnEntityType)cookie;
		switch (spawnInfo.EntityType)
		{
			case EntityType.Ship:
				if (!trackedEntities.ContainsKey (spawnInfo.EntityId))
				{
					ShipEntry entry = typeof(ShipEntry).GetProperty (spawnInfo.EntryName).GetValue (null, null) as ShipEntry;
					Debug.Log ("Player identified: " + spawnInfo.EntityId);
					AddShipInternal (entry, spawnInfo.SpawnPos, spawnInfo.SpawnRot, spawnInfo.EntityId);
				}
				break;
		}
	}

	public void OnPlayerDisconnected(object cookie)
	{
		NetSpawnEntityType spawnInfo = (NetSpawnEntityType)cookie;
		if (trackedEntities.ContainsKey (spawnInfo.EntityId))
		{
			trackedEntities [spawnInfo.EntityId].Unload ();
			trackedEntities.Remove (spawnInfo.EntityId);
		}
	}

	public void AddEntity()
	{

	}

	public ShipEntity AddLocalShip(ShipEntry ship, Vector3 spawnPos = new Vector3(), Vector3 spawnRot = new Vector3())
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
		Debug.Log ("mine: " + shipId);

		localShip = AddShipInternal (ship, spawnPos, spawnRot, shipId);
		Service.Network.BroadcastEntitySpawned (localShip);
		return localShip;
	}

	private ShipEntity AddShipInternal(ShipEntry ship, Vector3 spawnPos, Vector3 spawnRot, string uid)
	{
		ShipEntity entity = new ShipEntity (ship, uid, spawnPos, spawnRot);
		trackedEntities.Add (entity.Id, entity);
		return entity;
	}

	private void OnNetEntitySpawned(object cookie)
	{
		NetSpawnEntityType spawnInfo = (NetSpawnEntityType)cookie;
		switch (spawnInfo.EntityType)
		{
			case EntityType.Ship:
				ShipEntry entry = typeof(ShipEntry).GetProperty (spawnInfo.EntryName).GetValue (null, null) as ShipEntry;
				Debug.Log ("theirs: " + spawnInfo.EntityId);
				AddShipInternal (entry, spawnInfo.SpawnPos, spawnInfo.SpawnRot, spawnInfo.EntityId);
				break;
		}
	}

	private void OnNetTransformUpdated(object cookie)
	{
		NetEntityTransformType moveType = (NetEntityTransformType)cookie;
		if (trackedEntities.ContainsKey (moveType.EntityId))
		{
			trackedEntities [moveType.EntityId].Model.transform.position = moveType.Position;
			trackedEntities [moveType.EntityId].Model.transform.eulerAngles = moveType.Rotation;
		}
	}
}

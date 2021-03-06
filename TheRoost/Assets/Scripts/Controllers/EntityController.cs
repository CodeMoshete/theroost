﻿using UnityEngine;
using System.Collections;
using Models;
using Services;
using System.Collections.Generic;
using Events;
using Game.Controllers.Network.Types;
using Game.Enums;
using Models.Interfaces;
using System;
using MonoBehaviors;
using Utils;

public class EntityController
{
	private const string SHIP_PREFIX = "s*";
	private const string USER_PREFIX = "u*";
	private const string WEAPON_PREFIX = "w*";
	private const string TARGET_RIG_PREFIX = "t*";
	private const string MAP_PREFIX = "m";

	private Dictionary<string, int> localEntityTypeCounts;
	private Dictionary<string, Entity> trackedEntities;
	private Dictionary<string, Entity> trackedEnvironmentObjects;

	private Dictionary<string, IProjectile> projectiles;
	private int localProjectileCount;
	private List<IProjectile> projectilesToDestroy;

	private ShipEntity localShip;
	private TargetingEntity localTargeter;
	private MapEntity localMap;

	public EntityController()
	{
		localEntityTypeCounts = new Dictionary<string, int> ();
		trackedEntities = new Dictionary<string, Entity> ();
		trackedEnvironmentObjects = new Dictionary<string, Entity> ();
		projectiles = new Dictionary<string, IProjectile>();
		projectilesToDestroy = new List<IProjectile>();
		Service.Events.AddListener (EventId.NetPlayerConnected, OnPlayerConnected);
		Service.Events.AddListener (EventId.NetPlayerIdentified, OnPlayerIdentified);
		Service.Events.AddListener (EventId.NetPlayerDisconnected, OnPlayerDisconnected);
		Service.Events.AddListener (EventId.EntitySpawned, OnNetEntitySpawned);
		Service.Events.AddListener (EventId.EntityTransformUpdated, OnNetTransformUpdated);
		Service.Events.AddListener (EventId.EntityFiredLocal, FireLocalWeapon);
		Service.Events.AddListener (EventId.EntityFired, FireWeaponNetwork);
		Service.Events.AddListener (EventId.ApplicationExit, OnApplicationExit);
		Service.Events.AddListener (EventId.EntityHealthUpdate, OnEntityHealthChanged);
	}

	private void OnPlayerConnected(object cookie)
	{
		Service.Network.BroadcastIdentification (localShip, localMap);
		Service.Network.BroadcastEntitySpawned(localTargeter);
		foreach (KeyValuePair<string, Entity> pair in trackedEnvironmentObjects)
		{
			Service.Network.BroadcastEntitySpawned (pair.Value);
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
					ShipEntry entry = 
						typeof(ShipEntry).GetProperty (spawnInfo.EntryName).GetValue (null, null) as ShipEntry;
					Debug.Log ("Player identified: " + spawnInfo.EntityId);
					AddShipInternal (entry, spawnInfo.SpawnPos, spawnInfo.SpawnRot, spawnInfo.EntityId);
				}
				break;
			case EntityType.Map:
				if (localMap == null)
				{
					MapEntry map = typeof(MapEntry).GetProperty (spawnInfo.EntryName).GetValue (null, null) as MapEntry;
					Debug.Log ("Map identified: " + spawnInfo.EntityId);
					AddLocalMap (map);
				}
				break;
			case EntityType.EnvironmentObject:
				Debug.Log ("Environment Object identified: " + spawnInfo.EntityId);
				EnvironmentObjectEntry objEntry = 
					typeof(EnvironmentObjectEntry).GetProperty (spawnInfo.EntryName).GetValue (null, null) as EnvironmentObjectEntry;
				AddEnvironmentObjectInternal (objEntry, spawnInfo.SpawnPos, spawnInfo.SpawnRot, spawnInfo.EntityId);
				break;
		}
	}

	public void OnPlayerDisconnected(object cookie)
	{
		string entityId = (string)cookie;
		if (trackedEntities.ContainsKey (entityId))
		{
			trackedEntities [entityId].Unload ();
			trackedEntities.Remove (entityId);
		}
	}

	public void AddEntity()
	{

	}

	private void FireWeaponNetwork(object cookie)
	{
		NetEntityAttackType info = (NetEntityAttackType)cookie;
		ShipEntity ownerShip = trackedEntities[info.EntityId] as ShipEntity;
		WeaponPoint point = ownerShip.GetTurretById(info.WeaponPointId);
		FireWeaponInternal(
			info.ProjectileId,
			info.ProjectileEntryType, 
			info.EntityId, 
			info.TargetingEntityId, 
			ownerShip.Ship, 
			point,
			false
		);
	}

	public void FireLocalWeapon(object cookie)
	{
		WeaponFireData fireData = (WeaponFireData)cookie;
		ShipEntity ownerShip = trackedEntities[fireData.OwnerShipUID] as ShipEntity;
		WeaponPoint point = ownerShip.GetTurretById(fireData.WeaponPointId);
		string uid = USER_PREFIX + Service.Network.PlayerId + WEAPON_PREFIX + localProjectileCount;
		localProjectileCount++;
		FireWeaponInternal(
			uid,
			fireData.Weapon.Projectile.EntryName, 
			fireData.OwnerShipUID, 
			fireData.TargetReticleUID, 
			ownerShip.Ship, 
			point,
			true
		);

		Service.Network.BroadcastEntityAttack(
			uid,
			ownerShip, 
			trackedEntities[fireData.TargetReticleUID],
			point, 
			fireData.Weapon.Projectile
		);
	}

	private void FireWeaponInternal(
		string uid,
		string projectileEntryName, 
		string ownerId, 
		string targeterId, 
		ShipEntry entry, 
		WeaponPoint weaponPoint,
		bool isLocal)
	{
		ProjectileEntry projectileEntry = 
			typeof(ProjectileEntry).GetProperty (projectileEntryName).GetValue (null, null) as ProjectileEntry;
		
		IProjectile projectile = (IProjectile)Activator.CreateInstance(Type.GetType(projectileEntry.ClassName));

		if(!trackedEntities.ContainsKey(ownerId))
		{
			Debug.LogError("[EntityController] Not tracking entity " + ownerId);
			return;
		}

		if(!trackedEntities.ContainsKey(targeterId))
		{
			Debug.LogError("[EntityController] Not tracking targeter " + targeterId);
			return;
		}

		ShipEntity ownerShip = trackedEntities[ownerId] as ShipEntity;
		TargetingEntity ownerTarget = trackedEntities[targeterId] as TargetingEntity;
		projectile.Initialize(uid, ownerShip, projectileEntry, RegisterProjectileForDestroy, isLocal);
		projectile.Fire(ownerShip, weaponPoint, ownerTarget);
		projectiles.Add(uid, projectile);
	}

	public void RegisterProjectileForDestroy(IProjectile projectile)
	{
		if(projectiles.ContainsKey(projectile.Uid))
		{
			projectilesToDestroy.Add(projectile);
		}
	}

	public MapEntity AddLocalMap(MapEntry map)
	{
		string uid = MAP_PREFIX + map.EntryName;
		localMap = new MapEntity (map, uid, Vector3.zero, Vector3.zero);
		List<GameObject> pieceSpawners = UnityUtils.FindAllGameObjectContains<EnvironmentObjectSpawn>();
		for (int i = 0, count = pieceSpawners.Count; i < count; ++i)
		{
			EnvironmentObjectEntry entry = pieceSpawners [i].GetComponent<EnvironmentObjectSpawn> ().Entry;
			AddLocalEnvironmentObject (entry, pieceSpawners[i].transform.position, pieceSpawners[i].transform.eulerAngles);
			GameObject.Destroy (pieceSpawners [i]);
		}
		return localMap;
	}

	public EnvironmentObjectEntity AddLocalEnvironmentObject(
		EnvironmentObjectEntry entry, Vector3 spawnPos = new Vector3(), Vector3 spawnRot = new Vector3())
	{
		EnvironmentObjectEntity envObj = null;
		if (Service.Network.IsMaster)
		{
			if (!localEntityTypeCounts.ContainsKey (entry.ResourceName))
			{
				localEntityTypeCounts.Add (entry.ResourceName, 0);
			}
			else
			{
				localEntityTypeCounts [entry.ResourceName]++;
			}

			string resCount = localEntityTypeCounts [entry.ResourceName].ToString ();
			string objId = USER_PREFIX + Service.Network.PlayerId + SHIP_PREFIX + entry.ResourceName + resCount;
			Debug.Log ("Environment Object: " + objId + ", " + spawnPos.ToString() + ", " + spawnRot.ToString());

			envObj = AddEnvironmentObjectInternal (entry, spawnPos, spawnRot, objId);
			Service.Network.BroadcastEntitySpawned (envObj);
		}
		return envObj;
	}

	private EnvironmentObjectEntity AddEnvironmentObjectInternal(
		EnvironmentObjectEntry entry, Vector3 spawnPos, Vector3 spawnRot, string uid)
	{
		EnvironmentObjectEntity obj = new EnvironmentObjectEntity (entry, uid, spawnPos, spawnRot);
		trackedEntities.Add (uid, obj);
		trackedEnvironmentObjects.Add (uid, obj);
		return obj;
	}

	public TargetingEntity AddLocalTargetingEntity(GameObject aimingController)
	{
		string uid = USER_PREFIX + Service.Network.PlayerId + TARGET_RIG_PREFIX + "1";
		localTargeter = AddTargetingEntityInternal (aimingController.transform.position, 
			aimingController.transform.eulerAngles, 
			uid);
		Service.Network.BroadcastEntitySpawned(localTargeter);
		return localTargeter;
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

	private AIShipEntity AddAiShipInternal(ShipEntry ship, Vector3 spawnPos, Vector3 spawnRot, string uid)
	{
		AIShipEntity entity = new AIShipEntity (ship, uid, spawnPos, spawnRot);
		trackedEntities.Add (entity.Id, entity);
		return entity;
	}

	private TargetingEntity AddTargetingEntityInternal(Vector3 spawnPos, Vector3 spawnRot, string uid)
	{
		TargetingEntity entity = new TargetingEntity (uid, spawnPos, spawnRot);
		trackedEntities.Add (uid, entity);
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
			case EntityType.TargetingRig:
				Debug.Log ("their targeting reticle added: " + spawnInfo.EntityId);
				TargetingEntity enemyTarget = 
					AddTargetingEntityInternal (spawnInfo.SpawnPos, spawnInfo.SpawnRot, spawnInfo.EntityId);
				enemyTarget.Model.SetActive(false);
				break;
			case EntityType.EnvironmentObject:
				Debug.Log ("Environment Object added: " + spawnInfo.EntityId);
				EnvironmentObjectEntry objEntry = 
					typeof(EnvironmentObjectEntry).GetProperty (spawnInfo.EntryName).GetValue (null, null) as EnvironmentObjectEntry;
				AddEnvironmentObjectInternal (objEntry, spawnInfo.SpawnPos, spawnInfo.SpawnRot, spawnInfo.EntityId);
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

	private void OnNetTransformDestroyed(object cookie)
	{

	}

	// For updating entities that don't get updated by the network.
	public void Update(float dt)
	{
		foreach(KeyValuePair<string, IProjectile> pair in projectiles)
		{
			pair.Value.Update(dt);
		}

		int destCount = projectilesToDestroy.Count;
		for(int i = 0, count = destCount; i < count; i++)
		{
			projectiles.Remove(projectilesToDestroy[i].Uid);
			projectilesToDestroy[i].Unload();
		}

		if(destCount > 0)
		{
			projectilesToDestroy = new List<IProjectile>();
		}
	}

	private void OnApplicationExit(object cookie)
	{
		if (localShip != null)
		{
			Service.Network.BroadcastDisconnect (localShip);
		}
	}

	private void OnEntityHealthChanged(object cookie)
	{
		NetEntityHealthUpdateType healthUpdate = (NetEntityHealthUpdateType)cookie;
		if (trackedEntities.ContainsKey (healthUpdate.EntityId))
		{
			trackedEntities [healthUpdate.EntityId].SetHealth (healthUpdate.CurrentHealth);
		}
	}
}

using UnityEngine;
using System.Collections;

public class ProjectileEntry 
{
	public string ResourceName { get; private set; }
	public string EntryName { get; private set; }
	public string ClassName { get; private set; }
	public string HitPrefab { get; private set; }
	public float MoveSpeed { get; private set; }
	public float TrackingRate { get; private set; }
	public float Lifetime { get; private set; }
	public int Damage { get; private set; }
	public bool DestroyOnHit { get; private set; }

	/// EntryName must ALWAYS be the same as the property name.
	public ProjectileEntry(
		string resourceName, 
		string entryName, 
		string className,
		string hitPrefab,
		int damage,
		float moveSpeed, 
		float trackingRate,
		float lifetime,
		bool destroyOnHit)
	{
		ResourceName = resourceName;
		MoveSpeed = moveSpeed;
		EntryName = entryName;
		ClassName = className;
		HitPrefab = hitPrefab;
		TrackingRate = trackingRate;
		Lifetime = lifetime;
		Damage = damage;
		DestroyOnHit = destroyOnHit;
	}

	// Add more ship entries below here as we create them.
	public static ProjectileEntry Disruptor 
	{ 
		get
		{ 
			return new ProjectileEntry (
				"Models/Disruptor", 
				"Disruptor", 
				"Models.Projectiles.GenericProjectile",
				"FX/DisruptorHit",
				1,
				7.5f, 
				0.01f,
				2f,
				true);
		} 
	}

	public static ProjectileEntry Phaser 
	{ 
		get
		{ 
			return new ProjectileEntry (
				"Models/FedPhaser", 
				"Phaser", 
				"Models.Projectiles.GenericBeam", 
				"FX/BeamHit",
				1,
				10f, 
				3f,
				2f,
				false);
		} 
	}

	public static ProjectileEntry PhotonTorpedo 
	{ 
		get
		{ 
			return new ProjectileEntry (
				"Models/PhotonTorpedo", 
				"PhotonTorpedo", 
				"Models.Projectiles.GenericProjectile", 
				"FX/ExplosionHD",
				6,
				5f, 
				0.01f,
				5f,
				true);
		} 
	}
}
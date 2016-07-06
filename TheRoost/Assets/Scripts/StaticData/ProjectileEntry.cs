using UnityEngine;
using System.Collections;

public class ProjectileEntry 
{
	public string ResourceName { get; private set; }
	public string EntryName { get; private set; }
	public string ClassName { get; private set; }
	public float MoveSpeed { get; private set; }
	public float TrackingRate { get; private set; }
	public float Lifetime { get; private set; }

	/// EntryName must ALWAYS be the same as the property name.
	public ProjectileEntry(string resourceName, 
		string entryName, 
		string className, 
		float moveSpeed, 
		float trackingRate,
		float lifetime)
	{
		ResourceName = resourceName;
		MoveSpeed = moveSpeed;
		EntryName = entryName;
		ClassName = className;
		TrackingRate = trackingRate;
		Lifetime = lifetime;
	}

	// Add more ship entries below here as we create them.
	public static ProjectileEntry Disruptor 
	{ 
		get
		{ 
			return new ProjectileEntry (
				"Models/GalaxyClass", 
				"Disruptor", 
				"Models.Projectiles.GenericProjectile", 
				0.01f, 
				0.01f,
				5f);
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
				3f, 
				3f,
				2f);
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
				0.04f, 
				0.01f,
				5f);
		} 
	}
}
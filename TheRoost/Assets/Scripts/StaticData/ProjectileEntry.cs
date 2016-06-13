using UnityEngine;
using System.Collections;

public class ProjectileEntry 
{
	public string ResourceName { get; private set; }
	public string EntryName { get; private set; }
	public string ClassName { get; private set; }
	public float MoveSpeed { get; private set; }
	public float TrackingRate { get; private set; }

	/// EntryName must ALWAYS be the same as the property name.
	public ProjectileEntry(string resourceName, string entryName, string className, float moveSpeed, float trackingRate)
	{
		ResourceName = resourceName;
		MoveSpeed = moveSpeed;
		EntryName = entryName;
		ClassName = className;
		TrackingRate = trackingRate;
	}

	// Add more ship entries below here as we create them.
	public static ProjectileEntry Disruptor 
	{ 
		get
		{ 
			return new ProjectileEntry ("Models/GalaxyClass", "Disruptor", "GenericProjectile", 0.01f, 0.01f);
		} 
	}

	public static ProjectileEntry Phaser 
	{ 
		get
		{ 
			return new ProjectileEntry ("Models/GalaxyClass", "Phaser", "GenericBeam", 0.01f, 0.01f);
		} 
	}
}
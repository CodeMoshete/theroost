using UnityEngine;
using System.Collections;

public enum WeaponId
{
	PhaserStrip,
	DisruptorBank,
	PhotonTorpedo
}

public class WeaponEntry 
{
	public ProjectileEntry Projectile { get; private set; }
	public string EntryName { get; private set; }

	/// Shots per second.
	public float Cooldown { get; private set; }

	public string ResourceName { get; private set; }

	/// EntryName must ALWAYS be the same as the property name.
	public WeaponEntry(
		ProjectileEntry projectile, 
		string entryName, 
		string resourceName, 
		float cooldown)
	{
		Projectile = projectile;
		EntryName = entryName;
		ResourceName = resourceName;
		Cooldown = cooldown;
	}

	// Add more ship entries below here as we create them.
	public static WeaponEntry PhaserStrip 
	{ 
		get
		{ 
			return new WeaponEntry (
				ProjectileEntry.Phaser, 
				"PhaserStrip",
				"",
				3f);
		} 
	}

	public static WeaponEntry PhotonTorpedo 
	{ 
		get
		{ 
			return new WeaponEntry (
				ProjectileEntry.PhotonTorpedo, 
				"PhaserStrip",
				"",
				3f);
		} 
	}
}
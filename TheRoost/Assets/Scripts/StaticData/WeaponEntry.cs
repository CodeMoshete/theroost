using UnityEngine;
using System.Collections;

public enum WeaponId
{
	PhaserStrip,
	DisruptorBank,
	TorpedoLauncher
}

public class WeaponEntry 
{
	public ProjectileEntry Projectile { get; private set; }
	public string EntryName { get; private set; }

	/// The name of the C# class to instantiate for this weapon. See Scripts/Weapons for list of available weapons.
	public string ClassName { get; private set; }

	/// Shots per second.
	public float Cooldown { get; private set; }

	public string ResourceName { get; private set; }

	/// EntryName must ALWAYS be the same as the property name.
	public WeaponEntry(
		ProjectileEntry projectile, 
		string entryName, 
		string className, 
		string resourceName, 
		float cooldown)
	{
		Projectile = projectile;
		EntryName = entryName;
		ClassName = className;
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
				"GenericBeam",
				"",
				3f);
		} 
	}


}
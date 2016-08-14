using UnityEngine;
using System.Collections;

public class ShipEntry 
{
	public string ResourceName { get; private set; }
	public string EntryName { get; private set; }
	public float MoveSpeed { get; private set; }
	public float Health { get; private set; }

	/// EntryName must ALWAYS be the same as the property name.
	public ShipEntry(string resourceName, string entryName, float moveSpeed, float health)
	{
		ResourceName = resourceName;
		MoveSpeed = moveSpeed;
		EntryName = entryName;
		Health = health;
	}

	// Add more ship entries below here as we create them.
	public static ShipEntry GalaxyClass 
	{ 
		get
		{ 
			return new ShipEntry (
				"Models/GalaxyClass", 
				"GalaxyClass", 
				0.01f, 
				100f);
		} 
	}

	public static ShipEntry Gunship 
	{ 
		get
		{ 
			return new ShipEntry (
				"Models/Gunship", 
				"Gunship", 
				0.01f, 
				100f);
		} 
	}

	public static ShipEntry Interceptor
	{ 
		get
		{ 
			return new ShipEntry (
				"Models/Interceptor", 
				"Interceptor", 
				0.01f, 
				100f);
		} 
	}

	public static ShipEntry Skirmisher 
	{ 
		get
		{ 
			return new ShipEntry (
				"Models/Skirmisher", 
				"Skirmisher", 
				0.01f, 
				100f);
		} 
	}
}
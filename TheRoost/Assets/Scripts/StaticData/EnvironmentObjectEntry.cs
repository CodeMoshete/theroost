using UnityEngine;
using System.Collections;

public class EnvironmentObjectEntry 
{
	public string ResourceName { get; private set; }
	public string EntryName { get; private set; }
	public int Health { get; private set; }

	/// EntryName must ALWAYS be the same as the property name.
	public EnvironmentObjectEntry(string resourceName, string entryName, int health)
	{
		ResourceName = resourceName;
		EntryName = entryName;
		Health = health;
	}

	// Add more environment object entries below here as we create them.
	public static EnvironmentObjectEntry AsteroidHole 
	{ 
		get
		{ 
			return new EnvironmentObjectEntry (
				"EnvironmentObjects/AsteroidHole", 
				"AsteroidHole", 
				-1);
		} 
	}

	// Add more environment object entries below here as we create them.
	public static EnvironmentObjectEntry AsteroidMain 
	{ 
		get
		{ 
			return new EnvironmentObjectEntry (
				"EnvironmentObjects/AsteroidMain", 
				"AsteroidMain", 
				50);
		} 
	}
}
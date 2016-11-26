using UnityEngine;
using System.Collections;

public class MapEntry 
{
	public string ResourceName { get; private set; }
	public string EntryName { get; private set; }

	/// EntryName must ALWAYS be the same as the property name.
	public MapEntry(string resourceName, string entryName)
	{
		ResourceName = resourceName;
		EntryName = entryName;
	}

	// Add more ship entries below here as we create them.
	public static MapEntry BattleGround 
	{ 
		get
		{ 
			return new MapEntry (
				"Maps/map_battleground", 
				"BattleGround");
		} 
	}
}
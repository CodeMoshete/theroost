using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjectSpawn : MonoBehaviour 
{
	public enum EnvironmentObjects
	{
		AsteroidHole,
		AsteroidMain
	}

	[SerializeField]
	public EnvironmentObjects EnvironmentObject;

	public EnvironmentObjectEntry Entry
	{
		get
		{
			EnvironmentObjectEntry entry = null;
			switch(EnvironmentObject)
			{
				case EnvironmentObjects.AsteroidHole:
					entry = EnvironmentObjectEntry.AsteroidHole;
					break;
				case EnvironmentObjects.AsteroidMain:
					entry = EnvironmentObjectEntry.AsteroidMain;
					break;
			}
			return entry;
		}
	}
}

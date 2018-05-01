using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiShipObjectSpawn : MonoBehaviour 
{
	public enum AiObjects
	{
		Gunship
	}

	[SerializeField]
	public AiObjects AiObject;

	public ShipEntry Entry
	{
		get
		{
			ShipEntry entry = null;
			switch(AiObject)
			{
				case AiObjects.Gunship:
					entry = ShipEntry.Gunship;
					break;
			}
			return entry;
		}
	}
}

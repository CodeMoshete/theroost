using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using Controllers.Interfaces;

namespace Models
{
	public class AIShipEntity : ShipEntity, IUpdateObserver
	{
		public AIShipEntity(ShipEntry ship, string id, Vector3 spawnPos, Vector3 spawnRot) : 
			base( ship, id, spawnPos, spawnRot)
		{
			Service.FrameUpdate.RegisterForUpdate (this);
		}

		public void Update(float dt)
		{

		}
	}
}
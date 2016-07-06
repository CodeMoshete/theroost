//------------------------------------------------------------------------------
// NetEntityTransformType.cs
//
// Description:
//  Struct-like class that transports information from the netcontroller to the
//   battle controller.
//
// Author:
//       Benjamin Mosher <ben.mosher@disney.com>
//
// Copyright (c) 2015 Disney Interactive
//------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace Game.Controllers.Network.Types
{
	public class NetEntityAttackType
	{
		public string ProjectileId { get; private set; }
		public string EntityId { get; private set; }
		public string TargetingEntityId { get; private set; }
		public string ProjectileEntryType { get; private set; }
		public string WeaponPointId { get; private set; }

		public NetEntityAttackType (
			string projectileId,
			string entityId, 
			string targetingEntityId, 
			string projectileEntry, 
			string weaponPointId)
		{
			EntityId = entityId;
			WeaponPointId = weaponPointId;
			ProjectileEntryType = projectileEntry;
			TargetingEntityId = targetingEntityId;
			ProjectileId = projectileId;
		}
	}
}


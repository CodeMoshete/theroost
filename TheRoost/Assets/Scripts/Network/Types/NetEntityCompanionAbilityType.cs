//------------------------------------------------------------------------------
// NetEntityCompanionAbilityType.cs
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
	public class NetEntityCompanionAbilityType
	{
		public string EntityId { get; private set; }
		public string AbilityId { get; private set; }
		
		public NetEntityCompanionAbilityType (string entityId, string abilityId)
		{
			EntityId = entityId;
			AbilityId = abilityId;
		}
	}
}


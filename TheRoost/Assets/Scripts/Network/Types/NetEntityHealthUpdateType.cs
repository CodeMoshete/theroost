//------------------------------------------------------------------------------
// NetEntityHealthUpdateType.cs
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

namespace Game.Controllers.Network.Types
{
	public class NetEntityHealthUpdateType
	{
		public string EntityId { get; private set; }
		public string AttackerEntityId { get; private set; }
		public int CurrentHealth { get; private set; }

		public NetEntityHealthUpdateType (string entityId, string attackerEntityId, int currentHealth)
		{
			EntityId = entityId;
			CurrentHealth = currentHealth;
			AttackerEntityId = attackerEntityId;
		}
	}
}


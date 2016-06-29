//------------------------------------------------------------------------------
// NetworkEvents.cs
//
// Description:
//  Enum defining the possible network events that can occur.
//
// Author:
//       Benjamin Mosher <ben.mosher@disney.com>
//
// Copyright (c) 2014 Disney Interactive
//------------------------------------------------------------------------------

using System;

namespace Game.Controllers.Network.Enums
{
	public enum NetworkEvents
	{
		InitialHandshake,
		GameStartHandshake,
		PlayerHeroes,
		UpdateSync,
		PlayerMove,
		PlayerTarget,
		PlayerAbility,
		CompanionAbility,
		PlayerCapture,
		PlayerConnect,
		PlayerIdentify,
		PlayerDisconnect,
		EntityHealth,
		EntityDeath,
		EntitySpawned,
		EntityMoved,
		EntityAtPosition,
		EntityAttacked
	}
}


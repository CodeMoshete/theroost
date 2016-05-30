//------------------------------------------------------------------------------
// NetEntityTargetType.cs
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
	public class NetEntityTargetType
	{
		public string AttackerId { get; private set; }
		public string TargetId { get; private set; }
		
		public NetEntityTargetType (string attackerId, string targetId)
		{
			AttackerId = attackerId;
			TargetId = targetId;
		}
	}
}


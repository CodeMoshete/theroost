//------------------------------------------------------------------------------
// NetEntityMoveToType.cs
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
	public class NetEntityMoveToType
	{
		public string EntityId { get; private set; }
		public Vector3 MovePosition { get; private set; }
		
		public NetEntityMoveToType (string entityId, Vector3 movePosition)
		{
			EntityId = entityId;
			MovePosition = movePosition;
		}
	}
}


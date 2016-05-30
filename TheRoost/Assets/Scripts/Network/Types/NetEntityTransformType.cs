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
	public class NetEntityTransformType
	{
		public string EntityId { get; private set; }
		public Vector3 Position { get; private set; }
		public float Rotation { get; private set; }
		
		public NetEntityTransformType (string entityId, Vector3 position, float rotation)
		{
			EntityId = entityId;
			Position = position;
			Rotation = rotation;
		}
	}
}


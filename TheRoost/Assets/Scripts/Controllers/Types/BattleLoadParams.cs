//BattleLoadParams.cs
//Struct-like class for enforcing and transporting parameters to a scene controller

using System;
using Delegates;

namespace Controllers.Types
{
	public class BattleLoadParams
	{
		// Eventually we will pass our selected ship through to the next state.
		public Action OnBattleEnd { get; private set; }
		public ShipEntry Ship { get; private set; }
		public MapEntry Map { get; private set; }

		public BattleLoadParams (Action onBattleEnd, ShipEntry ship, MapEntry map)
		{
			OnBattleEnd = onBattleEnd;
			Ship = ship;
			Map = map;
		}
	}
}


//MainMenuLoadParams.cs
//Struct-like class for enforcing and transporting parameters to a scene controller

using System;
using Delegates;

namespace Controllers.Types
{
	public class MainMenuLoadParams
	{
		// Eventually we will pass our selected ship through to the next state.
		public Action<ShipEntry, MapEntry> OnBattleStart { get; private set; }

		public MainMenuLoadParams (Action<ShipEntry, MapEntry> onBattleStart)
		{
			OnBattleStart = onBattleStart;
		}
	}
}


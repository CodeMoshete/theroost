using UnityEngine;
using System.Collections;
using System;
using MonoBehaviors;

namespace Models.Interfaces
{
	public interface IProjectile 
	{
		string Uid { get; }
		void Initialize(
			string uid, 
			ShipEntity owner, 
			ProjectileEntry template, 
			Action<IProjectile> onDestroy, 
			bool isLocal);
		void Fire (ShipEntity source, WeaponPoint weapon, TargetingEntity targetEntity);
		void Update (float dt);
		void Unload ();
	}
}

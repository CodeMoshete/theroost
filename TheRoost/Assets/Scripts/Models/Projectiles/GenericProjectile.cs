﻿using UnityEngine;
using System.Collections;
using Models.Interfaces;
using System;
using MonoBehaviors;

namespace Models.Projectiles
{
	public class GenericProjectile : BaseProjectile
	{
		public override void Fire (ShipEntity source, WeaponPoint weapon, TargetingEntity targetEntity)
		{
			lifetimeLeft = projectileData.Lifetime;
			model.transform.position = weapon.transform.position;
			model.transform.LookAt(targetEntity.AimPosition);
		}

		public override void Update(float dt)
		{
			model.transform.Translate(0f, 0f, projectileData.MoveSpeed);
			lifetimeLeft -= dt;
			if(lifetimeLeft <= 0f)
			{
				ShowHitFX ();
				onDestroy(this);
			}
		}
	}
}

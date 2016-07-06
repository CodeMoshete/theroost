using UnityEngine;
using System.Collections;
using MonoBehaviors;
using Controllers.Interfaces;
using System;
using Models.Interfaces;
using Services;
using Events;

namespace Models
{
	public class Weapon : IUpdateObserver
	{
		private const string PROJECTILE_CLASS_PREFIX = "Models.Projectiles";

		public WeaponPoint WeaponPoint { get; private set; }
		private ShipEntity owner;
		private float currentCooldown;
		private Transform turretTransform;

		public Weapon(ShipEntity owner, WeaponPoint weapon)
		{
			this.owner = owner;
			WeaponPoint = weapon;
			turretTransform = WeaponPoint.gameObject.transform;

			// TODO: Load or select turret model if one exists in the WeaponEntry.

			Service.FrameUpdate.RegisterForUpdate (this);
			Service.Events.AddListener (EventId.PlayerAimed, AimWeapon);
		}

		public void Fire(Vector3 targetPos, string targetReticleUid)
		{
			if (currentCooldown <= 0f)
			{
				Vector3 turretPos = turretTransform.position;
				Vector3 vecToTarget = (targetPos - turretPos).normalized;
				float dotToTarget = Vector3.Dot (WeaponPoint.LookDirection, vecToTarget);
				if (dotToTarget >= WeaponPoint.DotFieldOfAttack)
				{
					WeaponFireData fireData = 
						new WeaponFireData(WeaponPoint.gameObject.name, targetReticleUid, WeaponPoint.Weapon, owner.Id);
					Service.Events.SendEvent(EventId.EntityFiredLocal, fireData);
				}
			}
		}

		public void Update(float dt)
		{
			if (currentCooldown > 0f)
			{
				currentCooldown -= dt;
			}
		}

		private void AimWeapon(object cookie)
		{
			// TODO: Point turrets in direction of controller reticle.
		}

		public void Unload()
		{
			Service.FrameUpdate.UnregisterForUpdate (this);
			Service.Events.RemoveListener(EventId.PlayerAimed, AimWeapon);
		}
	}
}

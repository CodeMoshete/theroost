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

		private ShipEntity owner;
		private WeaponPoint weaponPoint;
		private float currentCooldown;
		private Transform turretTransform;

		public Weapon(ShipEntity owner, WeaponPoint weapon)
		{
			this.owner = owner;
			weaponPoint = weapon;
			turretTransform = weaponPoint.gameObject.transform;

			// TODO: Load or select turret model if one exists in the WeaponEntry.

			Service.FrameUpdate.RegisterForUpdate (this);
			Service.Events.AddListener (EventId.PlayerAimed, AimWeapon);
			Service.Events.AddListener (EventId.EntityFired, FireWeapon);
		}

		public void Fire(Vector3 targetPos)
		{
			if (currentCooldown <= 0f)
			{
				Vector3 vecToTarget = (targetPos - turretTransform.position).normalized;
				float dotToTarget = Vector3.Dot (vecToTarget, weaponPoint.LookDirection);
				if (dotToTarget >= weaponPoint.DotFieldOfAttack)
				{
					Type projectileType = 
						Type.GetType (PROJECTILE_CLASS_PREFIX + weaponPoint.Weapon.Projectile.ClassName);
					IProjectile projectile = (IProjectile)Activator.CreateInstance (projectileType);
					projectile.Fire (owner, turretTransform.position, turretTransform.eulerAngles);
					currentCooldown = weaponPoint.Weapon.Cooldown;
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

		public void AimWeapon(object cookie)
		{
			// TODO: Point turrets in direction of controller reticle.
		}

		public void FireWeapon(object cookie)
		{
			
		}

		public void Unload()
		{
			Service.FrameUpdate.UnregisterForUpdate (this);
			Service.Events.RemoveListener(EventId.PlayerAimed, AimWeapon);
		}
	}
}

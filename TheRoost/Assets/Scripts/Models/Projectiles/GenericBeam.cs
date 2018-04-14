using System;
using System.Collections;
using Models.Interfaces;
using MonoBehaviors;
using Services;
using UnityEngine;
using Utils;

namespace Models.Projectiles
{
	public class GenericBeam : BaseProjectile
	{
		private const float DECAY_TIME_TOTAL = 0.25f;

		private float decayTimeLeft;
		private bool isDecaying;
		private TargetingEntity targetEntity;
		private Vector3 currentScale;
		private WeaponPoint weaponPoint;

		public override void Initialize(
			string uid,
			ShipEntity ownerShip,
			ProjectileEntry template, 
			Action<IProjectile> onDestroy,
			bool isLocal)
		{
			base.Initialize(uid, ownerShip, template, onDestroy, isLocal);
			currentScale = Vector3.one;
		}

		public override void Fire (ShipEntity source, WeaponPoint weapon, TargetingEntity targetEntity)
		{
			weaponPoint = weapon;
			this.targetEntity = targetEntity;
			lifetimeLeft = projectileData.Lifetime;
			model.transform.position = weapon.transform.position;
			model.transform.parent = source.Model.transform;
			model.transform.LookAt(targetEntity.AimPosition);
		}

		public override void Update(float dt)
		{
			if(!isDecaying)
			{
				model.transform.LookAt(targetEntity.AimPosition);
				model.transform.localScale = currentScale;
				currentScale.z += projectileData.MoveSpeed;
				lifetimeLeft -= dt;

				Vector3 vecToTarget = (targetEntity.AimPosition - weaponPoint.transform.position).normalized;
				float dotTest = Vector3.Dot(weaponPoint.LookDirection, vecToTarget);

				if(lifetimeLeft <= 0f || dotTest < weaponPoint.DotFieldOfAttack)
				{
					isDecaying = true;
					decayTimeLeft = DECAY_TIME_TOTAL;
				}
			}
			else
			{
				float xyScale = Mathf.Lerp(0f, 1f, decayTimeLeft / DECAY_TIME_TOTAL);
				currentScale.x = xyScale;
				currentScale.y = xyScale;
				model.transform.localScale = currentScale;
				decayTimeLeft -= dt;

				if(decayTimeLeft <= 0f)
				{
					onDestroy(this);
				}
			}
		}

		protected override void ShowHitFX ()
		{
			if (!string.IsNullOrEmpty (projectileData.HitPrefab))
			{
				Vector3 spawnPos = UnityUtils.FindGameObject (model, "FXRef").transform.position;
				Service.FXService.SpawnFX (projectileData.HitPrefab, spawnPos);
			}
		}
	}
}

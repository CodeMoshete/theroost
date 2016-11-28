using UnityEngine;
using System.Collections;
using Models.Interfaces;
using System;
using MonoBehaviors;
using Services;
using Utils;
using Game.MonoBehaviors;

namespace Models.Projectiles
{
	public class BaseProjectile : IProjectile
	{
		public string Uid { get; private set; }
		protected GameObject model;
		protected ProjectileEntry projectileData;
		protected Action<IProjectile> onDestroy;
		protected float lifetimeLeft;
		protected ShipEntity owner;
		protected bool isLocal;

		public virtual void Initialize(
			string uid,
			ShipEntity owner,
			ProjectileEntry template, 
			Action<IProjectile> onDestroy,
			bool isLocal)
		{
			Uid = uid;
			projectileData = template;
			this.onDestroy = onDestroy;
			model = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(template.ResourceName));
			this.owner = owner;

			model.AddComponent<ProjectileHitDetection>();
			model.GetComponent<ProjectileHitDetection>().RegisterListener(OnProjectileCollision);
		}

		protected virtual void OnProjectileCollision(GameObject other)
		{
			EntityRef entityRef = other.GetComponent<EntityRef> ();
				
			if (entityRef == null || owner != entityRef.Entity)
			{
				ShowHitFX ();
				ShowTargetHitFX (other);

				if (isLocal)
				{
					entityRef.Entity.SetHealth(entityRef.Entity.CurrentHealth - projectileData.Damage);
					Service.Network.BroadcastEntityHealthChanged (
						owner.Id, 
						entityRef.Entity.Id, 
						entityRef.Entity.CurrentHealth);
				}
			}
		}

		protected virtual void ShowHitFX()
		{
			// Override if explosion needs to be somewhere other than projectile position
			if (!string.IsNullOrEmpty (projectileData.HitPrefab))
			{
				Service.FXService.SpawnFX (projectileData.HitPrefab, model.transform.position);
			}
		}

		protected void ShowTargetHitFX(GameObject target)
		{
			if (Service.FXService.ExplosionDebrisMap.ContainsKey (target.tag))
			{
				Service.FXService.SpawnFX (Service.FXService.ExplosionDebrisMap [target.tag]);
			}
		}

		public virtual void Fire (ShipEntity source, WeaponPoint weapon, TargetingEntity targetEntity)
		{
			// For override only.
		}

		public virtual void Update(float dt)
		{
			// For override only
		}

		public virtual void Unload ()
		{
			GameObject.Destroy(model);
			model = null;
			projectileData = null;
		}
	}
}

using UnityEngine;
using System.Collections;
using Models.Interfaces;
using System;
using MonoBehaviors;

namespace Models.Projectiles
{
	public class BaseProjectile : IProjectile
	{
		public string Uid { get; private set; }
		protected GameObject model;
		protected ProjectileEntry projectileData;
		protected Action<IProjectile> onDestroy;
		protected float lifetimeLeft;

		public virtual void Initialize(
			string uid, 
			ProjectileEntry template, 
			Action<IProjectile> onDestroy, 
			bool isLocal)
		{
			Uid = uid;
			projectileData = template;
			this.onDestroy = onDestroy;
			model = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(template.ResourceName));

			if(isLocal)
			{
				model.AddComponent<ProjectileHitDetection>();
				model.GetComponent<ProjectileHitDetection>().RegisterListener(OnProjectileCollision);
			}
		}

		protected virtual void OnProjectileCollision(GameObject other)
		{
			// For override only.
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

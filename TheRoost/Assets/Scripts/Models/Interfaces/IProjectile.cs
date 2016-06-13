using UnityEngine;
using System.Collections;

namespace Models.Interfaces
{
	public interface IProjectile 
	{
		void Initialize(ProjectileEntry template);
		void Fire (ShipEntity source, Vector3 startPosition, Vector3 startEuler);
		void Unload ();
	}
}

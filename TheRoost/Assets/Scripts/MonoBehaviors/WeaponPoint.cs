using UnityEngine;
using System.Collections;
using System.Reflection;
using Utils;

namespace MonoBehaviors
{
	public class WeaponPoint : MonoBehaviour 
	{
		public string WeaponGroupId;
		public WeaponId WeaponId;
		public WeaponEntry Weapon 
		{
			get 
			{
				PropertyInfo propertyInfo = typeof(WeaponEntry).GetProperty(WeaponId.ToString());
				return (WeaponEntry)propertyInfo.GetValue(null, null);
			}
		}

		/// Values should be 0 - 180 degrees.
		public float FieldOfAttack;
		private float cachedDotFieldOfAttack;
		public float DotFieldOfAttack 
		{
			get 
			{
				if (FieldOfAttack > 0f && cachedDotFieldOfAttack == 0f)
				{
					cachedDotFieldOfAttack = Mathf.Cos (FieldOfAttack * Mathf.Deg2Rad);
				}
				return cachedDotFieldOfAttack;
			}
		}

		private readonly Vector3 DEFAULT_DIRECTION = new Vector3 (0f, 0f, 1f);
		public Vector3 LookDirection
		{
			get 
			{
				Vector3 dir = DEFAULT_DIRECTION;
				if (direction != null)
				{
					dir = (direction.transform.position - transform.position).normalized;
				}
				return dir;
			}
		}

		private Transform direction;
		public void Start()
		{
			direction = UnityUtils.FindGameObject (gameObject, "Direction").transform;
		}
	}
}
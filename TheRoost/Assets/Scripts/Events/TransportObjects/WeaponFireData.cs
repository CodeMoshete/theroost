using UnityEngine;
using System.Collections;

public class WeaponFireData
{
	public WeaponEntry Weapon { get; private set; }
	public string WeaponPointId { get; private set; }
	public string OwnerShipUID { get; private set; }
	public string TargetReticleUID { get; private set; }

	public WeaponFireData(string weaponPointId, string targetReticleUid, WeaponEntry weapon, string ownerShipUid)
	{
		WeaponPointId = weaponPointId;
		TargetReticleUID = targetReticleUid;
		Weapon = weapon;
		OwnerShipUID = ownerShipUid;
	}
}
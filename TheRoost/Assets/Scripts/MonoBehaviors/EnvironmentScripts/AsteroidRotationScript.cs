using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using Controllers.Interfaces;

public class AsteroidRotationScript : MonoBehaviour 
{
	public Vector3 RotationRate;

	public void Update () 
	{
		float dt = Time.deltaTime;
		transform.eulerAngles += (RotationRate * dt);
	}
}

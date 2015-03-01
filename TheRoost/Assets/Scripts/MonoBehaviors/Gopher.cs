using UnityEngine;
using System.Collections;

public class Gopher : MonoBehaviour {

	public float Speed = 0.5f;
	public float TurnRate = 0.5f;
	public GameObject Target;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(0f, 0f, Speed);

		if(Target == null)
		{
			this.transform.Rotate(0f, TurnRate, 0f);
		}
		else
		{
			this.transform.LookAt(Target.transform.position);
		}
	}
}

using UnityEngine;
using System.Collections;

public class SmallDebrisBehavior : MonoBehaviour {
	public float MAX_SPIN_RATE = 2.0f;
	public float MAX_VELOCITY = 0.2f;
	public float MIN_SCALE = 0.25f;
	public float MAX_SCALE = 2.0f;
	public float LIFESPAN = 10.0f;
	public float LIFESPAN_VARIANCE = 10.0f;

	private Vector3 SpinRate;
	private Vector3 Velocity;
	private Transform rotationPart;
	private float scheduledDestroyTime;
	private bool shouldDestroy;

	// Use this for initialization
	void Start ()
	{
		SpinRate = new Vector3(Random.Range(-MAX_SPIN_RATE,MAX_SPIN_RATE),
		                       Random.Range(-MAX_SPIN_RATE,MAX_SPIN_RATE),
		                       Random.Range(-MAX_SPIN_RATE,MAX_SPIN_RATE));

		Velocity = new Vector3(Random.Range(-MAX_VELOCITY,MAX_VELOCITY),
		                       Random.Range(-MAX_VELOCITY,MAX_VELOCITY),
		                       Random.Range(-MAX_VELOCITY,MAX_VELOCITY));

		rotationPart = this.transform.Find ("Debris").transform;
		float scaleFactor = Random.Range(MIN_SCALE,MAX_SCALE);
		this.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);

		shouldDestroy = false;
		scheduledDestroyTime = Time.fixedTime + LIFESPAN + Random.Range(0.0f, LIFESPAN_VARIANCE);
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.Translate(Velocity);

		if(rotationPart)
		{
			rotationPart.Rotate (SpinRate);
		}

		if(shouldDestroy)
		{
			if(this.transform.localScale.x <= 0.01)
			{
				Destroy(this.gameObject);
			}
			else
			{
				this.transform.localScale *= 0.99f;
			}
		}
	}

	void FixedUpdate()
	{
		if(Time.fixedTime >= scheduledDestroyTime)
		{
			shouldDestroy = true;
		}
	}
}

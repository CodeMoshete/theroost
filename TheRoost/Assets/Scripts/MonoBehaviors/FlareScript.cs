using UnityEngine;
using System.Collections;

public class FlareScript : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("OVRPlayerController");
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(player.transform.position);
		this.transform.Rotate(new Vector3(180,0,0));
	}
}

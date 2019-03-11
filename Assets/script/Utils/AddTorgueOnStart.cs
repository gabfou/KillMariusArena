using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorgueOnStart : MonoBehaviour {

	public float torque = 15;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddTorque(torque,ForceMode2D.Impulse);
	}
	
}

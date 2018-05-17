using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowStupide : MonoBehaviour {

	float dist;

	// Use this for initialization
	void Start () {
		dist = Camera.main.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = transform.position - new Vector3(0, -3, -dist);
		
	}
}

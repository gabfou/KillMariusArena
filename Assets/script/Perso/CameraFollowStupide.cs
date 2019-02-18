using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowStupide : MonoBehaviour {

	float dist = 10;
	public Transform follow = null;

	// Use this for initialization
	void Start () {
		dist = Camera.main.transform.position.z;
		if (follow = null)
			follow = transform;
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = follow.position - new Vector3(0, 0, -dist);
		
	}
}

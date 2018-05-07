using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowStupide : MonoBehaviour {

	public float dist = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = transform.position - new Vector3(0, -3, dist);
		
	}
}

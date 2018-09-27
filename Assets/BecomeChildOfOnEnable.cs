using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeChildOfOnEnable : MonoBehaviour {

	public Transform futureDad;
	// Use this for initialization
	void Start () {
		
	}
	
	private void OnEnable() {
        Vector3 tmp;
        tmp = transform.position;
		transform.parent = futureDad;
        transform.position = tmp;
	}
	// Update is called once per frame
	void Update () {
		
	}
}

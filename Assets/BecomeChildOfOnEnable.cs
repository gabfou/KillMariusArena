using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeChildOfOnEnable : MonoBehaviour {

	public Transform futureDad;
	// Use this for initialization
	void Start () {
		
	}
	
	private void OnEnable() {
		transform.parent = futureDad;
	}
	// Update is called once per frame
	void Update () {
		
	}
}

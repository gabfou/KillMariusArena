using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveAnimatorOnEnable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	private void OnEnable() {
		GetComponent<Animator>().enabled = false;
	}

	// Update is called once per frame
	void Update () {
		
	}
}

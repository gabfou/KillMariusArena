using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAnimatorIsMoving : MonoBehaviour {

	Rigidbody2D rbody;
	Animator animator;
	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		animator.SetBool("ismoving", rbody.velocity != Vector2.zero);
	}
}

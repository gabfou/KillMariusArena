using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroJump : PlayerController {

	// Use this for initialization
	Transform Cible = null;
	// Rigidbody2D rbody;

	void Start () {
		init();
		// facingRight = !facingRight;
		// rbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	protected override void FixedUpdate () {
		if (Cible == null)
			return ;
		move = Mathf.Sign((Cible.position - transform.position).x);
		base.FixedUpdate();
		if (Physics2D.Raycast(transform.position, new Vector3(move, 0, 0), 2, LayerMask.GetMask("Ground")))
			tryjump();
		if (Physics2D.Raycast(transform.position, Cible.position - transform.position, 2, LayerMask.GetMask("Player")))
			tryjump();
	}

	void Update()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		Cible = other.transform;
	}

	private void OnDisable()
	{
		Cible = null;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agro : PlayerController {

	// Use this for initialization
	Transform Cible = null;

	void Start () {
		init();
		// facingRight = !facingRight;
		// rbody = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		base.ouchtag = "bam";
	}

	// Update is called once per frame
	protected override void FixedUpdate () {
		if (Cible)
			move = Mathf.Sign((Cible.position - transform.position).x);
		else
			move = 0;
		base.FixedUpdate();
		if (Physics2D.Raycast(transform.position, new Vector3(move, 0, 0), 2, LayerMask.GetMask("Ground")))
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

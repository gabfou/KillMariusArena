using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Agro : PlayerController {

	// Use this for initialization
	Transform Cible = null;
	public bool StayOnGround = true;
	Collider2D realcol;

	void Start () {
		init();
		base.ouchtag = "bam";
		// facingRight = !facingRight;
		// rbody = GetComponent<Rigidbody2D>();
		realcol = GetComponents<Collider2D>().Where(c => !c.isTrigger).FirstOrDefault();
	}


	// Update is called once per frame
	protected override void FixedUpdate () {
		if (!base.cannotmove)
		{
			if (Cible)
			{
				move = Mathf.Sign((Cible.position - transform.position).x);
				if (!(Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 2, LayerMask.GetMask("Ground"))))
					move = 0;
			}
			else
				move = 0;
		}
		base.FixedUpdate();
		if (StayOnGround && Physics2D.Raycast(transform.position, new Vector3(move, 0, 0), 2, LayerMask.GetMask("Ground")))
			tryjump();

	}

	void Update()
	{
		
	}

	protected void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			Cible = other.transform;
	}
	override protected void OnTriggerStay2D(Collider2D other)
	{
		if (realcol.bounds.Intersects(other.bounds))
			base.OnTriggerStay2D(other);
	}

	// private void OnTriggerEnter2D(Collider2D other)
	// {
	// 	if (other.tag == "Player")
	// 		Cible = other.transform;
	// }

	private void OnDisable()
	{
		Cible = null;
	}
}

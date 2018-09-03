using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Agro : PlayerController {

    public enum DistanceBehavior {Free, JustFlee, Justcharge};

    Transform Cible = null;

    [Header("Agro setting")]
    public bool StayOnGround = true;
	Collider2D realcol;
    public float perfectdistancetocible = 0;
    public DistanceBehavior distanceBehavior = DistanceBehavior.Free;

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
                float distance = Vector2.Distance(Cible.position, transform.position);
                int sign = (distance > perfectdistancetocible) ? 1 : -1;
                if (distance < 0.1f)
                    move = 0;
                else if (!(Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 2, LayerMask.GetMask("Ground"))))
                    move = 0;
                else if (DistanceBehavior.Free != distanceBehavior && ((DistanceBehavior.Justcharge == distanceBehavior && sign == -1)
                                                                       || (DistanceBehavior.JustFlee == distanceBehavior && sign == 1)))
                    move = 0;
                else
                    move = sign * Mathf.Sign((Cible.position - transform.position).x);
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
 
	override protected void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			Cible = other.transform;
        if (realcol.bounds.Intersects(other.bounds))
            base.OnTriggerEnter2D(other);
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

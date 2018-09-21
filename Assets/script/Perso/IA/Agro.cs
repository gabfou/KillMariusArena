using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class Agro : PlayerController {

    public enum DistanceBehavior {Free, JustFlee, Justcharge};

    Transform Cible = null;

    [Header("Agro setting")]
    public bool StayOnGround = true;
	Collider2D realcol;
    public float perfectdistancetocible = 0;
    public DistanceBehavior distanceBehavior = DistanceBehavior.Free;
    public float MaxDistance = Mathf.Infinity;


    void Start () {
		init();
		base.ouchtag = "bam";
		realcol = GetComponents<Collider2D>().Where(c => !c.isTrigger).FirstOrDefault();
	}


	// Update is called once per frame
	protected override void FixedUpdate () {
		if (!base.cannotmove)
		{
            if (Cible)
			{
                float distance = Vector2.Distance(Cible.position, transform.position);
                if (distance > MaxDistance)
                {
                    Cible = null; // peut etre active reactive qaund respawn pres
                    return ;
                }
                int sign = (distance > perfectdistancetocible) ? 1 : -1;
                if (Mathf.Abs(distance - perfectdistancetocible) < 0.2f)
                    move = 0;
                else if (!(Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 2, LayerMask.GetMask("Ground"))))
                    move = 0;
                else if (DistanceBehavior.Free != distanceBehavior && ((DistanceBehavior.Justcharge == distanceBehavior && sign == -1)
                                                                       || (DistanceBehavior.JustFlee == distanceBehavior && sign == 1)))
                    move = 0;
                else
                    move = sign * Mathf.Sign((Cible.position - transform.position).x);

                if (move == 0)
                {
                    if (Cible.position.x > transform.position.x && facingRight)
                        Flip();
                    if (Cible.position.x < transform.position.x && !facingRight)
                        Flip();
                }
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

	CinemachineTargetGroup.Target t; 

	override protected void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Cible = other.transform;
			t.radius = 2;
			t.weight = 1;
			t.target = transform;
			if (!(!other || other.tag != "Player" || other.GetComponent<CinemachineTargetGroup>() == null))
			{
				// List<CinemachineTargetGroup.Target> targets =  other.GetComponent<CinemachineTargetGroup>().m_Targets.ToList();
				// targets.Add(t);
				// other.GetComponent<CinemachineTargetGroup>().m_Targets = targets.ToArray();
			}
		}
        if (realcol.bounds.Intersects(other.bounds))
            base.OnTriggerStay2D(other);
    }

	private void OnDisable()
	{
		if (Cible)
		{
			// List<CinemachineTargetGroup.Target> targets =  Cible.GetComponent<CinemachineTargetGroup>().m_Targets.ToList();
			// targets.Remove(t);
			// Cible.GetComponent<CinemachineTargetGroup>().m_Targets = targets.ToArray();
			Cible = null;
		}
	}
}

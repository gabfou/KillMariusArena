using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class Agro : PlayerController {

    public enum DistanceBehavior {Free, JustFlee, Justcharge, DontMove, Mounted};

    [HideInInspector] public Transform Cible = null;

    [Header("Agro setting")]
    public bool StayOnGround = true;
	Collider2D realcol;
    public float perfectdistancetocible = 0;
    public DistanceBehavior distanceBehavior = DistanceBehavior.Free;
    public float MaxDistance = Mathf.Infinity;
	float moveSmooth = 0;


	// public Sprite changeSpriteOnAgro;
	// public List<string> listOfAnimtOverwrite = new List<string>();


    void Start ()
	{
		init();
		base.ouchtag = "bam";
		realcol = GetComponents<Collider2D>().Where(c => !c.isTrigger).FirstOrDefault();
		if (!realcol)
			realcol = GetComponents<Collider2D>().LastOrDefault();
	}

	void MountedFixedUpdate(float distance)
	{
				Debug.Log("dsf2 " + move);
		float sign = Mathf.Sign((Cible.position - transform.position).x);
		if ((StayOnGround || (move != 0 && Mathf.Sign(move) != sign)) && !(Physics2D.Raycast(transform.position, new Vector3(Mathf.Sign(move), -1, 0), 4, groundLayer)))
			move = 0;
		// else if (move != 0 && Mathf.Sign(move) != sign && !(Physics2D.Raycast(transform.position, new Vector3(move, -2, 0), 2, groundLayer)))
		// 	move = 0;
		else if (Mathf.Sign(move) != sign)
			move = Mathf.Clamp(move + sign * 1f * Time.fixedDeltaTime, -1, 1);
		else
			move = sign;
		if (move == 0)
		{
			if (Cible.position.x > transform.position.x && facingLeft)
				Flip();
			if (Cible.position.x < transform.position.x && !facingLeft)
				Flip();
		}

		if (move != 0 && !StayOnGround && (Cible.position.y - 3 > transform.position.y)
			&& (Physics2D.Raycast(transform.position, new Vector3(move, 0, 0), 2, groundLayer)
				|| !(Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 2, groundLayer))))
			tryjump();
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void FixedUpdate ()
	{
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
				if (DistanceBehavior.Mounted == distanceBehavior)
				{
					MountedFixedUpdate(distance);
					return ;
				}
				if (DistanceBehavior.DontMove == distanceBehavior)
					move = 0;
                else if (Mathf.Abs(distance - perfectdistancetocible) < 0.2f)
                    move = 0;
                else if (StayOnGround && !(Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 2, groundLayer)))
                    move = 0;
                else if (DistanceBehavior.Free != distanceBehavior && ((DistanceBehavior.Justcharge == distanceBehavior && sign == -1)
																		|| (DistanceBehavior.JustFlee == distanceBehavior && sign == 1)))
                    move = 0;
                else
                    move = sign * Mathf.Sign((Cible.position - transform.position).x);

                if (move == 0)
                {
                    if (Cible.position.x > transform.position.x && facingLeft)
                        Flip();
                    if (Cible.position.x < transform.position.x && !facingLeft)
                        Flip();
                }

				if (move != 0 && !StayOnGround && (Cible.position.y - 3 > transform.position.y)
					&& (Physics2D.Raycast(transform.position, new Vector3(move, 0, 0), 2, groundLayer)
						|| !(Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 2, groundLayer))))
					tryjump();
            }
			else
				move = 0;
		}
		base.FixedUpdate();
		// if (Cible && base.cannotmove == false)
		// {
		// 		ContactFilter2D cf = new ContactFilter2D();
		// 		cf.SetLayerMask(gameObject.layer);
		// 		RaycastHit2D[] results = new RaycastHit2D[10];
		// 		if (col && col.Cast(transform.right, cf, results) > 0)
		// 		{
		// 			move += (results[0].transform.position.x > transform.position.x) ? -1 : 1;
		// 		}
		// }
	}

	void Update()
	{
		
	}

	CinemachineTargetGroup.Target t; 

	bool changesprite = false;
	override protected void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			changesprite = true;
			if (anim)
				anim.SetTrigger("Alert!");
			else
				Debug.Log(name + " pas D'anim");
			Cible = other.transform;
			// t.radius = 2;
			// t.weight = 1;
			// t.target = transform;
			// if (!(!other || other.tag != "Player" || other.GetComponent<CinemachineTargetGroup>() == null))
			// {
			// 	List<CinemachineTargetGroup.Target> targets =  other.GetComponent<CinemachineTargetGroup>().m_Targets.ToList();
			// 	targets.Add(t);
			// 	other.GetComponent<CinemachineTargetGroup>().m_Targets = targets.ToArray();
			// }
		}
		if (!realcol)
			Debug.Log(name + " pas de realcol");
        else if (realcol.IsTouching(other))
            base.OnTriggerStay2D(other);
    }

	override protected void GroundCheck()
	{
		base.GroundCheck();
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

	/// <summary>
	/// LateUpdate is called every frame, if the Behaviour is enabled.
	/// It is called after all Update functions have been called.
	/// </summary>
	void LateUpdate()
	{
		// if (changesprite)
		// {
		// 	 AnimatorClipInfo[] m_CurrentClipInfo = anim.GetCurrentAnimatorClipInfo(0);
		// 	if (changeSpriteOnAgro && m_CurrentClipInfo)
		// 		spriteRenderer.sprite = changeSpriteOnAgro;
		// 	// changesprite = false;
		// }
	}
}

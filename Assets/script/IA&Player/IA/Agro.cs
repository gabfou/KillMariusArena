using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UnityEngine.Events;

public class Agro : Character {

    public enum DistanceBehavior {Free, JustFlee, Justcharge, DontMove, Mounted, justTakeDamage};

    [HideInInspector] public Transform Cible = null;

    [Header("Agro setting")]
    public bool StayOnGround = true;
	Collider2D realcol;
    public float perfectdistancetocible = 0;
    public DistanceBehavior distanceBehavior = DistanceBehavior.Free;
    public float MaxDistance = Mathf.Infinity;
	public UnityEvent eventOnAgro;


	// public Sprite changeSpriteOnAgro;
	// public List<string> listOfAnimtOverwrite = new List<string>();

	public bool IsFacingPlayer()
	{
		if (Cible && ((Cible.position.x > transform.position.x && facingLeft)
			|| (Cible.position.x < transform.position.x && !facingLeft)))
			return false;
		return true;
	}

	protected void FacePlayer()
	{
		if (!IsFacingPlayer())
			Flip();
	}

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
		Vector2 dir = new Vector2(move, movey);
		Vector2 dirCible = ((Vector2)Cible.position - (Vector2)transform.position).normalized;
		bool isInSameDir = Vector2.Dot(dir.normalized, dirCible) > 0.5f;
		RaycastHit2D raycastHit2D;
		if (!flying && (StayOnGround || (dir.x != 0 && isInSameDir))
			&& (!(raycastHit2D = Physics2D.Raycast(transform.position, new Vector3(Mathf.Sign(dir.x), -1, 0), 4, groundLayer))
				|| (raycastHit2D.collider && raycastHit2D.collider.tag == ouchtag)))
			dir.x = 0;
		// else if (dir.x != 0 && Mathf.Sign(dir.x) != sign && !(Physics2D.Raycast(transform.position, new Vector3(dir.x, -2, 0), 2, groundLayer)))
		// 	dir.x = 0;
		else if (!isInSameDir)
		{
			dir.x = Mathf.Clamp(dir.x + dirCible.x * 0.7f * Time.fixedDeltaTime, -1, 1);
		}
		else
			dir.x = dirCible.x;
		if (dir.x == 0)
			FacePlayer();

		if (flying)
		{
			if (!isInSameDir)
				dir.y = Mathf.Clamp(dir.y + dirCible.y * 0.7f * Time.fixedDeltaTime, -1, 1);
			else
				dir.y = dirCible.y;
		}

		if (!flying && dir.x != 0 && !StayOnGround && (Cible.position.y - 3 > transform.position.y)
			&& (Physics2D.Raycast(transform.position, new Vector3(dir.x, 0, 0), 3, groundLayer)
				|| !(Physics2D.Raycast(transform.position, new Vector3(dir.x, -1, 0), 3, groundLayer))))
			tryjump();


		move = dir.x;
		movey = dir.y;
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void FixedUpdate ()
	{
		if (DistanceBehavior.justTakeDamage == distanceBehavior)
			return ;
		if (!base.cannotmove)
		{
            if (Cible && DistanceBehavior.DontMove != distanceBehavior)
			{
				RaycastHit2D raycastHit2D;
				float distance = Vector2.Distance(Cible.position, transform.position);
                if (distance > MaxDistance)
                {
                    Cible = null;// peut etre active reactive qaund respawn pres
                    return ;
                }
				int sign = (distance > perfectdistancetocible) ? 1 : -1;
				if (DistanceBehavior.Mounted == distanceBehavior)
				{
					MountedFixedUpdate(distance);
					return ;
				}
                if (Mathf.Abs(distance - perfectdistancetocible) < 0.2f)
                    move = 0;
                else if (StayOnGround && (!(raycastHit2D = Physics2D.Raycast(transform.position, new Vector3(Mathf.Sign(move), -1, 0), 4, groundLayer))
				|| (raycastHit2D.collider && raycastHit2D.collider.tag == ouchtag)))
                    move = 0;
                else if (DistanceBehavior.Free != distanceBehavior && ((DistanceBehavior.Justcharge == distanceBehavior && sign == -1)
																		|| (DistanceBehavior.JustFlee == distanceBehavior && sign == 1)))
                    move = 0;
                else
                    move = sign * Mathf.Sign((Cible.position - transform.position).x);

				if (move != 0 && !StayOnGround && (Cible.position.y + 3 > transform.position.y)
					&& (Physics2D.Raycast(transform.position, new Vector3(move, 0, 0), 2, groundLayer)
						|| !(Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 3, groundLayer))
						|| ((raycastHit2D = Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 5, groundLayer)).collider && raycastHit2D.collider.tag == ouchtag)))
					tryjump();
            }
			else
				move = 0;
			if (Cible && move == 0)
				FacePlayer();
		}
		base.FixedUpdate();
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
			eventOnAgro.Invoke();
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
}

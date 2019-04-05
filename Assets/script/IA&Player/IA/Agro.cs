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
	public string tagCible = "Player";
    [ConditionalHide("flying", true)]
	public bool StayOnGround = true;
	Collider2D realcol;
    public float perfectdistancetocible = 0;
    public DistanceBehavior distanceBehavior = DistanceBehavior.Free;
    public float MaxDistance = Mathf.Infinity;
	public UnityEvent eventOnAgro;
	public bool alwaysFaceCible = false;
	[HideInInspector] public float id = -1;

	public bool IsFacingPlayer()
	{
		if (Cible && ((Cible.position.x > transform.position.x && facingLeft)
			|| (Cible.position.x < transform.position.x && !facingLeft)))
			return false;
		return true;
	}

	public void FacePlayer()
	{
		if (!IsFacingPlayer())
			Flip();
	}

	// private void OnEnable()
	// {
	// 	// checking if already killed if yes kill it again;
	// 	if (gameObject.scene.isLoaded == false)
	// 	{
	// 		id = transform.position.sqrMagnitude;
	// 		if (GameManager.instance.save.listOfObjectAlreadyUse.Contains(id))
	// 		{
	// 			GameObject.Destroy(gameObject);
	// 			return ;
	// 		}
	// 	}
	// }

    void Start ()
	{
		init();
		realcol = GetComponents<Collider2D>().Where(c => !c.isTrigger).FirstOrDefault();
		if (!realcol)
			realcol = GetComponents<Collider2D>().LastOrDefault();
	}

	void pseudoPathfinding() // jump or not 
	{
		if (StayOnGround)
			return ;
		RaycastHit2D raycastHit2D;
		if (move != 0 && (Cible.position.y + 3 > transform.position.y)
			&& (Physics2D.Raycast(transform.position, new Vector3(move, 0, 0), 2, groundLayer)
				|| !(Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 3, groundLayer))
				|| ((raycastHit2D = Physics2D.Raycast(transform.position, new Vector3(move, -1, 0), 5, groundLayer)).collider && raycastHit2D.collider.tag == ouchtag)))
			tryjump();
		else if (transform.position.y - Cible.position.y > 4 && Mathf.Abs(transform.position.x - Cible.position.x) < 2)
			tryGoUnder();
		else if (transform.position.y - Cible.position.y < -4 && Mathf.Abs(transform.position.x - Cible.position.x) < 2)
			tryjump();
	}

	void MountedFixedUpdate(float distance)
	{
		Vector2 dir = new Vector2(move, movey);
		Vector2 dirCible = ((Vector2)Cible.position - (Vector2)transform.position).normalized;
		bool isInSameDir = Vector2.Dot(dir.normalized, dirCible) > 0.5f;
		RaycastHit2D raycastHit2D;
		if (!flying && (StayOnGround || (dir.x != 0 && (Mathf.Sign(dir.x) != Mathf.Sign(dirCible.x))))
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

		pseudoPathfinding();


		move = dir.x;
		movey = dir.y;
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void FixedUpdate ()
	{
		if (DistanceBehavior.justTakeDamage == distanceBehavior)
			return ;
		if (!base.cannotmove && Cible)
		{
			if (alwaysFaceCible == true)
				FacePlayer();
            if (DistanceBehavior.DontMove != distanceBehavior)
			{
				RaycastHit2D raycastHit2D;
				float distance = Mathf.Abs(Cible.position.x - transform.position.x);
                if (distance > MaxDistance)
                {
                    Cible = null;// peut etre active reactive quand respawn pres
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

				pseudoPathfinding();
				if (move == 0)
					FacePlayer();
            }
			else
				move = 0;	
		}
		base.FixedUpdate();
	}

	void Update()
	{
	}

	CinemachineTargetGroup.Target t; 

	bool changesprite = false;
	override protected void OnTriggerEnter2D(Collider2D other)
	{
		if (!realcol)
			Debug.Log(name + " pas de realcol");
        else if (realcol.IsTouching(other))
            base.OnTriggerEnter2D(other);
    }

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!Cible && other.tag == tagCible)
		{
			changesprite = true;
			if (anim)
				anim.SetTrigger("Alert!");
			else
				Debug.Log(name + " pas D'anim");
			
			Cible = other.transform;
			eventOnAgro.Invoke();
		}
	}

	override protected void GroundCheck()
	{
		base.GroundCheck();
	}

	override protected void Die()
	{
		//if (id >= 0)
		//	GameManager.instance.save.listOfObjectAlreadyUseButNotSave.Add(id);
		base.Die();
	}

	private void OnDisable()
	{
		if (Cible)
		{
			Cible = null;
		}
	}
}

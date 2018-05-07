using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingCharge : MonoBehaviour {

	public float chargejumppower = 10;
	public float coefspeed = 2f;
	PlayerController ps;
	public float cd = 2;
	float	actualcd;
	float	TimeToCoolDown = 1f;
	Animator anim;
	Collider2D OuchZone;

	// Use this for initialization
	void Start () {
		ps = GetComponentInParent<PlayerController>();
		anim = GetComponentInParent<Animator>();
		OuchZone = GetComponentInChildren<Collider2D>();
	}

	private void OnEnable()
	{
		isInJumpingCharging = false;
		actualcd = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (actualcd > 0)
			actualcd -= Time.deltaTime;
	}

	bool isInJumpingCharging = false;

	IEnumerator JumpingCharging()
	{
		isInJumpingCharging = true;
		ps.cannotmove = true;
		ps.tryjump(chargejumppower);
		OuchZone.enabled = true;
		while (true)
		{
			if (ps.grounded && ps.canJump)
				break ;
			ps.Move(ps.move * coefspeed);
			yield return new WaitForEndOfFrame();
		}
		OuchZone.enabled = false;
		ps.Move(0);
		anim.SetBool("TakingTimeToCoolDown", true);
		yield return new WaitForSeconds(TimeToCoolDown);
	
		anim.SetBool("TakingTimeToCoolDown", false);
		ps.cannotmove = false;
		actualcd = cd;
		isInJumpingCharging = false;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (actualcd > 0 || isInJumpingCharging)
			return ;
		if (other.tag == "Player" && ps.grounded && ps.canJump)
			StartCoroutine(JumpingCharging());
	}
}

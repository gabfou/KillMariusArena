using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
		OuchZone = GetComponentsInChildren<Collider2D>(true).Where(c => c.tag == "ouch").FirstOrDefault();
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
		if (ps.istapping && ps.TakingDamage == true)
		{
			StopCoroutine("JumpingCharging");
			anim.SetBool("istapping", false);
			OuchZone.enabled = false;
			ps.istapping = false;
			ps.cannotmove = false;
			isInJumpingCharging = false;
			anim.SetBool("TakingTimeToCoolDown", false);
		}
	}

	bool isInJumpingCharging = false;

	IEnumerator JumpingCharging()
	{
		float move = (ps.facingRight) ? -1 : 1;
		ps.Move(move * 2);
		isInJumpingCharging = true;
		ps.cannotmove = true;
		ps.istapping = true;
		ps.tryjump(chargejumppower);
		anim.SetBool("istapping", true);
		OuchZone.enabled = true;
        if (ps.TappingClip)
            ps.audiosource.PlayOneShot(ps.TappingClip, ps.tappingVolume);
		while (true)
		{
			ps.Move(move * coefspeed);
			if (ps.grounded && ps.canJump)
				break ;
			ps.Move(ps.move * coefspeed);
			yield return new WaitForEndOfFrame();
		}
		anim.SetBool("istapping", false);
		OuchZone.enabled = false;
		ps.istapping = false;
		ps.Move(0);
		anim.SetBool("TakingTimeToCoolDown", true);
		yield return new WaitForSeconds(TimeToCoolDown);
	
		anim.SetBool("TakingTimeToCoolDown", false);
		ps.cannotmove = false;
		actualcd = cd;
		isInJumpingCharging = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		OnTriggerStay2D(other);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (actualcd > 0 || isInJumpingCharging)
			return ;
		if (other.tag == "Player" && ps.grounded && ps.canJump && !ps.istapping)
			StartCoroutine(JumpingCharging());
	}
}

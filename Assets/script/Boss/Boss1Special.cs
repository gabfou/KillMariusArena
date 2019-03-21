using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Special : MonoBehaviour {

	Transform Cible;
	Animator animator;
	int initialife;
	Agro agro;
	float nbReCible = 1;
	// Use this for initialization
	void Start ()
	{
		agro = GetComponent<Agro>();
		initialife = agro.life;
		animator = GetComponent<Animator>();
		GetComponentInChildren<Throwthing>().modifProjectile = modif;
	}
	
	public void modif(Rigidbody2D rb)
	{
		rb.GetComponent<AutoReCible>().nbReCible = nbReCible;
	}

	void Update()
	{
		if (!Cible)
			Cible = GameManager.instance.player.transform;
		if (initialife - agro.life > 4)
		{
			nbReCible++;
			initialife = agro.life;
		}
		if (agro.life < 1)
			animator.SetBool("CASSOS", true);
	}

	public void ActivateTrigger()
	{
		if (agro.life > 0)
			animator.SetTrigger("trigger");
	}

	void LateUpdate () {
		if (Mathf.Sign(Cible.position.x - transform.position.x) != Mathf.Sign(transform.localScale.x))
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}

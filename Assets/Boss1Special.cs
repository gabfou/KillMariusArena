using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Special : MonoBehaviour {

	Transform Cible;
	Animator animator;
	int initialife;
	AutoReCible autoReCible;
	Agro agro;
	// Use this for initialization
	void Start ()
	{
		agro = GetComponent<Agro>();
		initialife = agro.life;
		Cible = GameManager.instance.player.transform;
		animator = GetComponent<Animator>();
		autoReCible = GetComponentInChildren<Throwthing>().Projectile.GetComponent<AutoReCible>();
	}
	
	void Update()
	{
		if (initialife - agro.life > 4)
		{
			autoReCible.nbReCible++;
			initialife = agro.life;
		}
	}

	void LateUpdate () {
		if (Mathf.Sign(Cible.position.x - transform.position.x) != Mathf.Sign(transform.localScale.x))
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReCible : MonoBehaviour {

	Transform Cible;
	public float nbReCible = 1;
	public enum ReCibleBehaviour {Boomerang, CiblePlayer};
	public ReCibleBehaviour reCibleBehaviour = ReCibleBehaviour.Boomerang;
	bool isInRecible = false;
	public float timeToStop = 1;
	Rigidbody2D rb;
	// Use this for initialization
	void Start ()
	{
		Cible = GameManager.instance.player.transform;
		rb = GetComponent<Rigidbody2D>();
	}
	
	IEnumerator ReCibleBoomerang()
	{
		float time = 0;
		Vector2 velocity = rb.velocity;
		while (timeToStop > time)
		{
			float t = (timeToStop - time * 2) / timeToStop;
			time += Time.deltaTime;
			rb.velocity = velocity * t;
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator ReCiblePlayer()
	{
		float time = 0;
		Vector2 velocity = rb.velocity;
		Vector2 newDir = Vector2.zero;
		while (timeToStop > time)
		{
			float t = (timeToStop - time * 2) / timeToStop;
			time += Time.deltaTime;
			if (t > 0)
				rb.velocity = velocity * t;
			else
			{
				if (newDir == Vector2.zero)
					newDir = (Cible.position - transform.position).normalized;
				rb.velocity = velocity.magnitude * newDir * -t;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (nbReCible > 0 && isInRecible == false
			&& Vector2.Distance((Vector2)transform.position + rb.velocity.normalized, Cible.position) > Vector2.Distance(transform.position, Cible.position))
		{
			isInRecible = true;
			if (reCibleBehaviour == ReCibleBehaviour.Boomerang)
				StartCoroutine(ReCibleBoomerang());
			else if (reCibleBehaviour == ReCibleBehaviour.CiblePlayer)
				StartCoroutine(ReCiblePlayer());;
			nbReCible--;
		}
	}
}

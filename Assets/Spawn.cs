using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public GameObject gameObject = null;
	public float cd =1;
	public float decalage = 0;
	float actualcd;
	public bool onlyIfLastDie = false;
	GameObject last = null;
	void Start () {
		actualcd = cd;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (decalage > 0)
		{
			decalage -= Time.fixedDeltaTime;
			return ;
		}
		actualcd -= Time.fixedDeltaTime;
		if (actualcd <= 0 && (onlyIfLastDie == false || last == null))
		{
			actualcd = cd;
			last = GameObject.Instantiate(gameObject, transform.position, Quaternion.identity);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStupid : MonoBehaviour {

	PlayerController pc;
	Transform sprite;
	int sign = 1;
	float timesincelast = 0;
	public float cadence = 0.1f;

	// Use this for initialization
	void Start () {
		pc = GetComponent<PlayerController>();
		sprite = GetComponentInChildren<SpriteRenderer>().transform;
		sprite.Rotate(Vector3.forward, -10);
	}
	
	// Update is called once per frame
	void Update () {
		timesincelast += Time.deltaTime;
		if (timesincelast > cadence)
		{
			sprite.transform.eulerAngles = new Vector3(0, 0, sign * 10);
			sign = -sign;
			timesincelast = 0;
		}
	}
}

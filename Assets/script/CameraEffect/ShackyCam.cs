
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShackyCam : MonoBehaviour {

	public	float		power = 0.7f;
	public	float		duration = 1.0f;
	Transform	cam;
	public	bool		ShouldShake = false;

	private	Vector3	startPosition;
	private	float	initialDuration;
	// Use this for initialization
	void Start () {
		cam = Camera.main.transform;
		startPosition = cam.eulerAngles;
		initialDuration = duration;
	}
	
	// Update is called once per frame
	void Update () {
		if (ShouldShake) {
			if (duration > 0) {
				cam.eulerAngles = startPosition + Random.insideUnitSphere * power;
				duration -= Time.deltaTime;
			}
			else {
				ShouldShake = false;
				duration = initialDuration;
				cam.eulerAngles = startPosition;
			}
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpZone : MonoBehaviour {

	// Use this for initialization
	public Vector3 exit;
	Collider2D zonetp;

	void Start () {
		if (!GetComponent<Collider2D>())
		{
			zonetp = gameObject.AddComponent<Collider2D>();
			zonetp.isTrigger = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			other.gameObject.transform.position = exit;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int num;
	public Sprite sp;
	SpriteRenderer spr;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player")
		{
			// other.GetComponent<PlayerController>().lastCheckpoint = transform.positio
			GameManager.instance.save.lastCheckpoint = transform.position;
			if (!spr)
			{
				spr = GetComponent<SpriteRenderer>();
				spr.sprite = sp;
			}
		}
	}
}

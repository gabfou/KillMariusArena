using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int num;
	public Sprite sp;
	SpriteRenderer spr;
	public GameObject replaceBy = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player")
		{
			GameManager.instance.save.lastCheckpoint = transform.position;
			GameManager.instance.save.replaceBy = replaceBy;
			if (!spr)
			{
				spr = GetComponent<SpriteRenderer>();
				if (spr)
					spr.sprite = sp;
			}
		}
	}
}

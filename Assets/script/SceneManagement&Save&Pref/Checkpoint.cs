using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public int num;
	public Sprite sp;
	SpriteRenderer spr;
	public GameObject replaceBy = null;
	public GameObject parent = null;
	public GameObject CamToActive = null;

	bool asBeenActivated = false;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			if (asBeenActivated == false)
			{
				asBeenActivated = true;
				GameManager.instance.player.life = GameManager.instance.player.maxLife;
				if (GameManager.instance?.life)
            		GameManager.instance.life.text = GameManager.instance.player.life.ToString();
			}
			GameManager.instance.save.lastCheckpoint = transform.position;
			GameManager.instance.save.replaceBy = replaceBy;
			GameManager.instance.save.parent = parent;
			if (!spr)
			{
				spr = GetComponent<SpriteRenderer>();
				if (spr)
					spr.sprite = sp;
			}
		}
	}
}

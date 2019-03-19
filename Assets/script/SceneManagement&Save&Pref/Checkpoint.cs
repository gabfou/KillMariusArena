using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public int num;
	public Sprite sp;
	SpriteRenderer spr;
	public int replaceBy = -1;
	public GameObject parent = null;
	public GameObject CamToActive = null;

	bool asBeenActivated = false;
	float id = -1;

	private void Start()
	{
		// check if as already been acivated
		id = transform.position.sqrMagnitude;
		if (GameManager.instance.save.listOfObjectAlreadyUse.Contains(id))
		{
			asBeenActivated = true;
			changeSprite();
		}
	}

	void changeSprite()
	{
		if (!spr)
		{
			spr = GetComponent<SpriteRenderer>();
			if (spr)
				spr.sprite = sp;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			GameManager.instance.save.lastCheckpoint = transform.position;
			GameManager.instance.save.replaceBy = replaceBy;
			GameManager.instance.save.parent = parent;
			changeSprite();
			if (asBeenActivated == false)
			{
				asBeenActivated = true;
				GameManager.instance.player.life = GameManager.instance.player.maxLife;
				if (GameManager.instance?.life)
            		GameManager.instance.life.text = GameManager.instance.player.life.ToString();
				GameManager.instance.save.listOfObjectAlreadyUse.Add(id);
				GameManager.instance.save.save();
			}

		}
	}
}

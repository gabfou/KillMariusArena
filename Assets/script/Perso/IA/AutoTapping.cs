﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTapping : MonoBehaviour {

	// Use this for initialization

    Animator anim;
    PlayerController pc;

    private void Update()
    {
        
    }
	IEnumerator Tapping()
    {
        pc.istapping = true;
        anim.SetBool("istapping", true);
        // move = transform.position.x - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).x; // tape ducoter de la sourie (en gros la ca sert a rien)
        // if (!istapping && move > 0 && !facingRight)
        //     Flip();
        // else if (!istapping && move < 0 && facingRight)
        //     Flip();

        yield return new WaitForSeconds(0.5f);
        StopTapping();
    }

    void StopTapping()
    {
        anim.SetBool("istapping", false);
        pc.istapping = false;
    }

	void Start () {
		anim = GetComponentInParent<Animator>();
		pc = GetComponentInParent<PlayerController>();
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && pc.istapping == false)
        {
			StartCoroutine(Tapping());
        }
	}
}

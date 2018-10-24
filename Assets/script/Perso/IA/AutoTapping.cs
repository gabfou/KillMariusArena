using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTapping : MonoBehaviour {

	// Use this for initialization

    Animator anim;
    public float timeofanimbeforetap = 0.25f;
    public float timeafter = 0.25f;

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
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            if (pc.istapping == false)
                StopTapping();
            yield return new WaitForEndOfFrame();
        }
        if (pc.TappingClip && pc.istapping)
            pc.audiosource.PlayOneShot(pc.TappingClip, pc.tappingVolume);
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            if (pc.istapping == false)
                StopTapping();
            yield return new WaitForEndOfFrame();
        }
        StopTapping();
    }

    void StopTapping()
    {
        anim.SetBool("istapping", false);
        StopCoroutine(Tapping());
        pc.istapping = false;
    }

	void Start () {
		anim = GetComponentInParent<Animator>();
		pc = GetComponentInParent<PlayerController>();
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && pc.istapping == false && pc.IsOuchstun == false)
        {
			StartCoroutine(Tapping());
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Grille : MonoBehaviour {
	public enum IsOpen {Open, Close, mid};
	// Use this for initialization
	SpriteRenderer spriteRenderer;
	public float autoclose = -1;

	public IsOpen  isOpen = IsOpen.Close;
	public AudioClip openClip;
	public AudioClip closeClip;
	public float volume = 0.5f;

	float decal;
	AudioSource audioSource;
	Collider2D c;
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		decal = (isOpen == IsOpen.Close) ? 0 : spriteRenderer.size.y;
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	IEnumerator openCoroutine()
	{
		isOpen = IsOpen.mid;
		while (decal < spriteRenderer.size.y)
		{
			float tmp = Time.deltaTime * spriteRenderer.size.y / 2;
			decal +=  tmp;
			transform.position += new Vector3(0, tmp);
			yield return new WaitForEndOfFrame();
		}
		isOpen = IsOpen.Open;
	}

	public void open()
	{
		if (audioSource)
			audioSource.PlayOneShot(openClip, volume);
		StartCoroutine("openCoroutine");
		StopCoroutine("closeCoroutine");
	}

	IEnumerator closeCoroutine()
	{
		isOpen = IsOpen.mid;
		while (decal > 0)
		{
			float tmp = Time.deltaTime * spriteRenderer.size.y / 2;
			decal -=  tmp;
			transform.position -= new Vector3(0, tmp);
			yield return new WaitForEndOfFrame();
		}
		isOpen = IsOpen.Close;
	}

	public void close()
	{
		if (audioSource)
			audioSource.PlayOneShot(closeClip, volume);
		StartCoroutine("closeCoroutine");
		StopCoroutine("openCoroutine");
	}

	public void openOrClose()
	{
		if (isOpen == IsOpen.Open)
			close();
		else if (isOpen == IsOpen.Close)
			open();
	}
}

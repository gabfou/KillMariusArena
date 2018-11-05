using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lever : MonoBehaviour {

	public GameObject[]	activate;
	public List<string> activateurTag = new List<string>(){"Player"};
	bool asbBeenActivated = false;
	public bool oneTimeUse = true;
	public float delayInSec = -1;
	float timeSinceLast = 4;
	public bool ActiveOnStay = false;
	
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (oneTimeUse == false)
			timeSinceLast += Time.deltaTime;
	}

	public void activator()
	{
		Animator a = null;
		if (oneTimeUse)
			asbBeenActivated = true;
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		foreach(GameObject g in activate)
		{
			if (g.tag == "porte")
				g.GetComponent<Grille>().open();
			else if (g.tag == "CaBouge")
				g.GetComponent<CaBouge>().isDeplacing = true;
			else if ((a = g.GetComponent<Animator>()) == true && a.enabled == false)
				a.enabled = true;
			else if (a  && g.tag == "trigger")
				a.SetTrigger("trigger");
		}
	}
	
	IEnumerator delay()
	{
		yield return new WaitForSeconds(delayInSec);
		activator();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Debug.Log(other.name);
		if (!asbBeenActivated && activateurTag.Contains(other.tag) && timeSinceLast > 3)
		{
			timeSinceLast = 0;
			if (delayInSec <= 0)
				activator();
			else
				StartCoroutine(delay());
			
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (ActiveOnStay)
			OnTriggerEnter2D(other);
	}
}

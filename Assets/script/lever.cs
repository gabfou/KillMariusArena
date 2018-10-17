using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lever : MonoBehaviour {

	public GameObject[]	activate;
	public List<string> activateurTag = new List<string>(){"Player"};
	bool asbBeenActivated = false;
	// Use this for initialization
	void Start () {
		
	}

	public void activator()
	{
		Animator a = null;
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
		}
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.name);
		if (!asbBeenActivated && activateurTag.Contains(other.tag))
			activator();
	}
}

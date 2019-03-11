using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class EnableOnTrigger : MonoBehaviour {

	public string limitToTag = "";
	public GameObject cible = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (limitToTag == "" || limitToTag == other.tag)
			cible.SetActive(true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class enableAnimatorOnTrigger : MonoBehaviour {


	public List<string> tagAccept;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (tagAccept.Count == 0 || tagAccept.Contains(other.tag))
			GetComponent<Animator>().enabled = true;
	}
}

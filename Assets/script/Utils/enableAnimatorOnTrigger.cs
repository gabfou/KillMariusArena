using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class enableAnimatorOnTrigger : MonoBehaviour {


	public List<string> tagAccept;
	public bool autoDestroyLastTrigger = false;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (tagAccept.Count == 0 || tagAccept.Contains(other.tag))
		{
			GetComponent<Animator>().enabled = true;
			if (autoDestroyLastTrigger)
				GameObject.Destroy(GetComponents<Collider2D>().Where(c => c.isTrigger).LastOrDefault());
		}
	}
}

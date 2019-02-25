using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

	public string nexTSceneName;
	public float WaitInSeconds = 2;
	public string tagAccepted = "Player";
	// Use this for initialization
	
	IEnumerator WaitForNext()
	{
		yield return new WaitForSeconds(WaitInSeconds);
		SceneManager.LoadScene(nexTSceneName);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == tagAccepted)
		{
			StartCoroutine(WaitForNext());
		}

	}
}

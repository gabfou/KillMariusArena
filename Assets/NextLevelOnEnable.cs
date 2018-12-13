using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelOnEnable : MonoBehaviour
{
	public float WaitInSeconds = 2;
    public string nexTSceneName;
	
	IEnumerator WaitForNext()
	{
		yield return new WaitForSeconds(WaitInSeconds);
		SceneManager.LoadScene(nexTSceneName);
	}

	void OnEnable()
	{
			StartCoroutine(WaitForNext());
    }
}

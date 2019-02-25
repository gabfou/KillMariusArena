using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelOnEnable : MonoBehaviour
{
	public float WaitInSeconds = 2;
    public string nexTSceneName;
	AsyncOperation ao;
	
	IEnumerator WaitForNext()
	{
		AsyncOperation ao;

		yield return null; //wait one frame in case the scene just loaded
		ao = SceneManager.LoadSceneAsync(nexTSceneName);
		ao.allowSceneActivation = false;
		yield return new WaitForSeconds(WaitInSeconds);
		ao.allowSceneActivation = true;
	}

	void OnEnable()
	{
		
		StartCoroutine(WaitForNext());
    }

}

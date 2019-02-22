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
		yield return new WaitForSeconds(WaitInSeconds);
		if (GameManager.instance != null)
			GameManager.instance.save.levelChangeReinit();
		ao.allowSceneActivation = true;
	}

	void OnEnable()
	{
		ao = SceneManager.LoadSceneAsync(nexTSceneName);
		ao.allowSceneActivation = false;
		StartCoroutine(WaitForNext());
    }

	private void Update() {
		Debug.Log(ao.progress);
	}
}

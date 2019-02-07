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
		AsyncOperation ao = SceneManager.LoadSceneAsync(nexTSceneName);
		ao.allowSceneActivation = false;
		yield return new WaitForSeconds(WaitInSeconds);
		GameManager.instance.save.levelChangeReinit();
		ao.allowSceneActivation = true;
	}

	void OnEnable()
	{
			StartCoroutine(WaitForNext());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelOnEnable : PrepNextLevel
{
	public float WaitInSeconds = 2;
	
	protected IEnumerator WaitForNext()
	{
		yield return null; //wait one frame in case the scene just loaded
		PreLoadScene();
		yield return new WaitForSeconds(WaitInSeconds);
		ActiveScene();
	}

	void OnEnable()
	{
		StartCoroutine(WaitForNext());
    }

	override public void stopLoad(Scene scene)
	{
		StopAllCoroutines();
		base.stopLoad(scene);
	}
}

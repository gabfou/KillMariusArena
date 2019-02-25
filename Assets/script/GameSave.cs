using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSave {
	public int level = 0;
	public int lastlevel = 0;
	public Vector2 lastCheckpoint= Vector2.zero;
	public GameObject replaceBy = null;
	public GameObject parent = null;
	public GameObject camToActive = null;
	[HideInInspector]public string SceneName;


	public void levelChangeReinit()
	{
		replaceBy = null;
		parent = null;
		camToActive = null;
		lastCheckpoint= Vector2.zero;
		SceneName = SceneManager.GetActiveScene().name;
	}
}

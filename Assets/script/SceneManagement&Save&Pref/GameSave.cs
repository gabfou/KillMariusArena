using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSave {
	public Vector2 lastCheckpoint= Vector2.zero;
	public GameObject replaceBy = null;
	public GameObject parent = null;
	public GameObject camToActive = null;
	[HideInInspector]public string SceneName = "";
	[HideInInspector] public List<float> listOfObjectAlreadyUse = new List<float>();
	[HideInInspector] public List<float> listOfObjectAlreadyUseButNotSave = new List<float>();
	public Difficulty difficulty;


	public void levelChangeReinit(string sceneName = "")
	{
		listOfObjectAlreadyUse.Clear();
		lastCheckpointReinit();
		replaceBy = null;
		parent = null;
		camToActive = null;
		lastCheckpoint= Vector2.zero;
		SceneName = (sceneName == "") ? SceneManager.GetActiveScene().name : sceneName;
	}

	public void lastCheckpointReinit()
	{
		listOfObjectAlreadyUseButNotSave.Clear();
	}

	public void save()
	{
		listOfObjectAlreadyUse.AddRange(listOfObjectAlreadyUseButNotSave);
		listOfObjectAlreadyUseButNotSave.Clear();
		PlayerPrefs.SetString("save", JsonUtility.ToJson(this));
	}

	public void load()
	{
		Debug.Log(PlayerPrefs.GetString("save"));
		string str;
		if ((str = PlayerPrefs.GetString("save")) != null)
			JsonUtility.FromJsonOverwrite(str, this);
		else
			return ;
		lastCheckpointReinit();
		Debug.Log(SceneName);
		SceneManager.LoadScene(SceneName);
	}
}

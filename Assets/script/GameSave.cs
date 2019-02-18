using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameSave {
	public int level = 0;
	public int lastlevel = 0;
	public Vector2 lastCheckpoint= Vector2.zero;
	public GameObject replaceBy = null;
	public GameObject parent = null;


	void Update () {
		
	}

	public void levelChangeReinit()
	{

		replaceBy = null;
		parent = null;
		lastCheckpoint= Vector2.zero;
	}
}

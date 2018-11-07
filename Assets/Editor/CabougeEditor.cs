using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(CaBouge)), CanEditMultipleObjects]
public class CabougeInspector : Editor {

	CaBouge cabouge;

	// List<Vector2>	list = new List<Vector2>();

	void OnEnable()
	{ 
		cabouge = target as CaBouge;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
	}

	public void OnSceneGUI()
	{
		for (int i = 0; i < cabouge.listOfPassage.Count; i++) // pour l instant c est stupide (meme boxBound pour toute la liste)
		{
			cabouge.listOfPassage[i] = Handles.PositionHandle(cabouge.listOfPassage[i], Quaternion.identity);
		}
	}
}


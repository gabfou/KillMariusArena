using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(CaBouge)), CanEditMultipleObjects]
public class CabougeInspector : Editor {

	CaBouge cabouge;
	int actualSize;

	// List<Vector2>	list = new List<Vector2>();

	void OnEnable()
	{ 
		cabouge = target as CaBouge;
		actualSize = cabouge.listOfPassage.Count;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
	}

	public void OnSceneGUI()
	{
		if (actualSize > cabouge.listOfPassage.Count)
			actualSize = cabouge.listOfPassage.Count;
		for (int i = 0; i < cabouge.listOfPassage.Count; i++)
		{
			if (actualSize < i)
			{
				actualSize = i;
				cabouge.listOfPassage[i] = cabouge.transform.position;
			}
			cabouge.listOfPassage[i] = Handles.PositionHandle(cabouge.listOfPassage[i], Quaternion.identity);
		}
	}
}


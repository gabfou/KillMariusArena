using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(CaBouge)), CanEditMultipleObjects]
public class CabougeInspector : Editor {

	CaBouge cabouge;
	int actualSize;
	Vector3 actualPosition;

	GUIStyle styleText;
	// List<Vector2>	list = new List<Vector2>();

	void OnEnable()
	{ 
		cabouge = target as CaBouge;
		actualSize = cabouge.listOfPassage.Count;
		styleText = new GUIStyle();
        styleText.normal.textColor = Color.white;
		actualPosition = cabouge.transform.position;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
	}

	public void OnSceneGUI()
	{
		if (actualPosition != cabouge.transform.position && !Application.isPlaying)
		{
			Vector2 diff = cabouge.transform.position - actualPosition;
			for (int i = 0; i < cabouge.listOfPassage.Count; i++)
				cabouge.listOfPassage[i] += diff;
			actualPosition = cabouge.transform.position;
		}

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
			Handles.Label(cabouge.listOfPassage[i], i.ToString(), styleText);
		}
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathfinderGrid))]
public class PathfinderGridEditor : Editor
{
	PathfinderGrid pg;

	void OnEnable()
	{ 
		pg = target as PathfinderGrid;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Make Grid !"))
		{
			pg.GenerateGrid();
		}
	}
}
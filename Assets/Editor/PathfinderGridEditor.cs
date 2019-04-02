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
		if (GUILayout.Button("Try Grid !"))
		{
			Pathfinder.Node start = pg.allNodes[Random.Range(0, pg.allNodes.Count)];
			Pathfinder.Node end = pg.allNodes[Random.Range(0, pg.allNodes.Count)];
			Debug.Log("start = " + start + " end = " + end);
			List<Pathfinder.Node> path = pg.GetAStar(start, end, -1);
			// path.ForEach(p => Debug.Log(p));

		}
	}
}
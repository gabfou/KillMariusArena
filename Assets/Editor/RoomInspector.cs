using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(Room))]
public class RoomInspector : Editor {

	Room room;

	List<BoxBoundsHandle>	boxBoundsHandle = new List<BoxBoundsHandle>();
	BoxBoundsHandle	boxBoundsHandle2 = new BoxBoundsHandle();
	Bounds		bounds;

	void OnEnable()
	{ 
		room = target as Room;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Make room !"))
		{
			Debug.Log("making room ...");
		}

		// if (room.roomPrefab != null)
		// {
		// 	roomCreate = room.roomPrefab.GetComponent< RoomCreate >();
		// 	EditorGUILayout.LabelField("bidon: " + roomCreate.bidon);
		// }
	}

	public void OnSceneGUI()
	{
		int i = 0;
		foreach(var exit in room.exitlist) // pour l instant c est stupide (meme boxBound pour toute la liste)
		{
			if (i >= boxBoundsHandle.Count)
				boxBoundsHandle.Add(new BoxBoundsHandle());
			boxBoundsHandle[i].size = exit.size;
			boxBoundsHandle[i].center = exit.center + (Vector2)room.transform.position;
			boxBoundsHandle[i].SetColor(Color.red);
			boxBoundsHandle[i].DrawHandle();
			exit.size = boxBoundsHandle[i].size;
			exit.center = boxBoundsHandle[i].center - room.transform.position;
			i++;
		}
		boxBoundsHandle2.size = room.bounds.size;
		boxBoundsHandle2.center = room.bounds.center + room.transform.position;
		boxBoundsHandle2.SetColor(Color.blue);
		boxBoundsHandle2.DrawHandle();
		room.bounds.size = boxBoundsHandle2.size;
		room.bounds.center = boxBoundsHandle2.center - room.transform.position;
	}
}

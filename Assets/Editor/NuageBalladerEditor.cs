using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.IMGUI.Controls;

// [CustomEditor(typeof(NuageBaladeur))]
// public class NuageBaladerEditor : Editor {

// 	NuageBaladeur nb;

// 	BoxBoundsHandle	boxBoundsHandle = new BoxBoundsHandle();
// 	Bounds		bounds;

// 	void OnEnable()
// 	{ 
// 		nb = target as NuageBaladeur;
// 	}

// 	public override void OnInspectorGUI()
// 	{
// 		DrawDefaultInspector();


// 		// if (room.roomPrefab != null)
// 		// {
// 		// 	roomCreate = room.roomPrefab.GetComponent< RoomCreate >();
// 		// 	EditorGUILayout.LabelField("bidon: " + roomCreate.bidon);
// 		// }
// 	}

// 	public void OnSceneGUI()
// 	{
//         // boxBoundsHandle.size = exit.size;
//         // boxBoundsHandle.center = exit.center + (Vector2)nb.transform.position;
//         // boxBoundsHandle.SetColor(Color.blue);
//         // boxBoundsHandle.DrawHandle();
//         // exit.size = boxBoundsHandle.size;
//         // exit.center = boxBoundsHandle.center - room.transform.position;


// 		boxBoundsHandle.size = nb.nuageZone.size;
// 		boxBoundsHandle.center = nb.nuageZone.center + nb.transform.position;
// 		boxBoundsHandle.SetColor(Color.blue);
// 		boxBoundsHandle.DrawHandle();
// 		nb.nuageZone.size = boxBoundsHandle.size;
// 		nb.nuageZone.center = boxBoundsHandle.center - nb.transform.position;
// 	}
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum side {right, left, down, up};

[System.Serializable]
public class exit {

	public Vector2 center;
	public Vector2 size = Vector2.one;
	[HideInInspector] public Vector2	pos;
	public side side = 0;
	[HideInInspector] public Room parent = null;


	
	// public static bool operator==(exit e1, exit e2){
	// 	if (e1.center == e2.center && e1.size == e2.size && e1.pos == e2.pos && e1.side == e2.side)
	// 		return true;
	// 	return false;
	// }

	// public static bool operator!=(exit e1, exit e2){
	// 	return !(e1 == e2);
	// }
}

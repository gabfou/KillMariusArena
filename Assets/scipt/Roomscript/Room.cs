using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	// Use this for initialization
	public List<exit> exit;
	GeneratorUtil gu;

	void Start () {
		gu = Camera.main.GetComponent<GeneratorUtil>();
		foreach(exit e in exit)
		{
			e.pos = e.center + (Vector2)transform.position;
		}
		gu.listporte.AddRange(exit);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SupExit(exit e)
	{
		int i = -1;

		while (++i < exit.Count)
		{
			if (exit[i] == e)
				exit.RemoveAt(i);
		}
	}
}

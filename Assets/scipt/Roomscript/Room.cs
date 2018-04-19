using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	// Use this for initialization
	public List<exit> exit;
	[HideInInspector] public bool initdone = false;
	GeneratorUtil gu;
	public Bounds bounds;

	void Start () {
		init();
	}

	public void init()
	{
		if (initdone == true)
			return ;
		gu = Camera.main.GetComponent<GeneratorUtil>();
		foreach(exit e in exit)
		{
			e.pos = e.center + (Vector2)transform.position;
			e.parent = this;
		}
		bounds.center += transform.position;
		gu.listporte.AddRange(exit);
		gu.listroomplaced.Add(this);
		initdone = true;
		// Debug.Log("dafuq2 " + Time.timeSinceLevelLoad);
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

	// private void OnDrawGizmos()
	// {
	// 	Gizmos.DrawCube(bounds.center, bounds.size);
	// }
}

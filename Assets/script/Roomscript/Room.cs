using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	// Use this for initialization
	public List<exit> exitlist;
	[HideInInspector] public bool initdone = false;
	GeneratorUtil gu;
	public Bounds bounds;
	public List<TpZone> listTpZone;

	void Start () {
		init();
	}
	void OnEnable () {
		init();
	}

	public void init()
	{
		if (initdone == true)
			return ;
		gu = Camera.main.GetComponent<GeneratorUtil>();
		foreach(exit e in exitlist)
		{
			e.pos = e.center + (Vector2)transform.position;
			e.parent = this;
		}
		foreach(TpZone t in listTpZone)
		{
			Debug.Log("fdsf");
			t.transform.position += transform.position;
		}

		bounds.center += transform.position;
		gu.listporte.AddRange(exitlist);
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

		while (++i < exitlist.Count)
		{
			if (exitlist[i] == e)
				exitlist.RemoveAt(i);
		}
	}

	public TpZone addTpFromExit(exit e)
	{
		TpZone r = GameObject.Instantiate<TpZone>(gu.TpZonePrefab, e.pos, Quaternion.identity, transform);
		if (e.parent == false || e.parent.initdone == false)
		r.transform.position += (Vector3)e.center + transform.position;
		if (listTpZone == null)
			listTpZone = new List<TpZone>();

		r.transform.localScale = new Vector3(0.9f,0.9f,0.9f);
		r.transform.position -= (Vector3)gu.getVecInDirOfSide(e.side, 0.1f);
		listTpZone.Add(r);
		// r.transform.localScale = e.size;
		return r;
	}

	// private void OnDrawGizmos()
	// {
	// 	Gizmos.DrawCube(bounds.center, bounds.size);
	// }
}

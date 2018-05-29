using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneratorUtil : MonoBehaviour {

	public List<Room> listroom;
	/*[HideInInspector]*/ public List<exit> listporte;
	[HideInInspector] public List<Room> listroomplaced;
	public TpZone TpZonePrefab;
	// Use this for initializationgu
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("l"))
		{
			Debug.Log("trying to add a room");
			if (addRoom())
				Debug.Log("success");
			else
				Debug.Log("fail");
		}
		if (Input.GetKeyDown("k"))
		{
			Debug.Log("trying to add a lot of room");
			for (int i = 0; i < 100; i++)
			{
				if (addRoom())
					Debug.Log("success");
				else
					Debug.Log("fail");
			}
		}
		if (Input.GetKeyDown("j"))
		{
			Debug.Log("cleaning");
			ExitCleaner();
		}
	}

	bool addRoom()
	{
		if (listroom.Count < 1)
		{
			Debug.Log("listroom pas set in generatorUtil");
			return false;
		}
		if (listporte.Count < 1)
		{
			Debug.Log("listporte pas set in generatorUtil");
			return false;
		}

		int r2 = Random.Range(0, listporte.Count);
		int i2 = r2;
		int debut2 = 0;
		while (i2 != r2 || debut2 == 0)
		{
			debut2 = 1;
			exit p = listporte[i2];

			int r = Random.Range(0, listroom.Count);
			int i = r;
			int debut = 0;
			while (i != r || debut == 0)
			{
				debut = 1;
				if (trytofitroom(p, listroom[i]))
					return true;
				i = (i + 1 < listroom.Count) ? i + 1 : 0;
			}
			i2 = (i2 + 1 < listporte.Count) ? i2 + 1 : 0;
		}
		return false;
	}

	bool CheckIfRoomFit(Bounds b, Vector3 offset)
	{
		// return true;
		b.center += offset;
		foreach(Room r in listroomplaced)
		{
			// Debug.Log("b = " + b + " r + " + r.bounds);
			if (b.Intersects(r.bounds))
				return false;
		}
		return true;
	}

	bool IsExitSideAndSizeMatch(exit a, exit b)
	{
		return (a.size == b.size && ((int)a.side % 2 == 0) ? a.side + 1 == b.side : a.side - 1 == b.side);
	}

	bool trytofitroom(exit p, Room room)
	{
		int r = Random.Range(0, room.exitlist.Count);
		int i = r;
		int debut = 0;
		while (i != r || debut == 0)
		{
			debut = 1;
			if (IsExitSideAndSizeMatch(room.exitlist[i], p) && CheckIfRoomFit(room.bounds, p.pos - room.exitlist[i].center))
			{
				Room newr = GameObject.Instantiate(room, p.pos - room.exitlist[i].center, Quaternion.identity); // apparament le start de la nouvelle room n est pas apeller avant la fin de la fonction faudrait savoir si c est pas juste du hasard

				// adding and attach the tpzone
				TpZone t1 = newr.addTpFromExit(room.exitlist[i]);
				TpZone t2 = p.parent.addTpFromExit(p);
				t1.exit = (Vector2)t1.transform.position + getVecInDirOfSide(room.exitlist[i].side, 1.2f);
				t1.exitroom = p.parent;
				t2.exit = (Vector2)t2.transform.position + getVecInDirOfSide(p.side, 1.2f);
				t2.exitroom = room.exitlist[i].parent;

				// nettoyage
				newr.SupExit(room.exitlist[i]);
				newr.init();
				p.parent.SupExit(p);
				listporte.Remove(p);
				// RemoveExitInLp(room.exitlist[i]);// supprime le exit d ici : pas necessaire pour l instant vu que je suprime celui de la room
				return true;
			}
			i = (i + 1 < room.exitlist.Count) ? i + 1 : 0;
		}
		return false;
	}

	public Vector2 getVecInDirOfSide(side side, float value)
	{
		Vector3 tmp = Vector3.zero;

		switch (side)
		{
			case side.left:
				tmp = new Vector3(-value, 0, 0);
				break;
			case side.right:
				tmp = new Vector3(value, 0, 0);
				break;
			case side.down:
				tmp = new Vector3(0, -value, 0);
				break;
			case side.up:
				tmp = new Vector3(0, value, 0);
				break;

		}
		return (tmp);
	}

	public Vector2 addInDirOfSide(side side, Vector2 o, float value)
	{
		return (o + getVecInDirOfSide(side, value));
	}

	void ExitCleaner() // don t work if exit are not carre
	{
		listporte.RemoveAll((p) => CheckIfRoomFit(new Bounds(addInDirOfSide(p.side, p.center, p.size.x), p.size), Vector3.zero));
	}

	void fillexit(Room r)
	{
		
	}
}

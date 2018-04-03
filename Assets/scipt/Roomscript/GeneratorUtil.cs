using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorUtil : MonoBehaviour {

	public List<Room> listroom;
	public List<exit> listporte;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("q"))
		{
			Debug.Log("trying to add a room");
			if (addRoom())
				Debug.Log("success");
			else
				Debug.Log("fail");
		}
		if (Input.GetKeyDown("w"))
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
		Debug.Log(listporte.Count);
		exit p = listporte[0]; // Placeholder

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
		return false;
	}

	bool trytofitroom(exit p, Room room)
	{
		int r = Random.Range(0, room.exit.Count);
		int i = r;
		int debut = 0;
		while (i != r || debut == 0)
		{
			debut = 1;
			if (room.exit[i].size == p.size && ((int)room.exit[i].side % 2 == 0) ? room.exit[i].side + 1 == p.side : room.exit[i].side - 1 == p.side
				&& true)
			{
				Room newr;
				newr = GameObject.Instantiate(room, p.pos - room.exit[i].center, Quaternion.identity);
				listporte.Remove(p);
				// RemoveExitInLp(room.exit[i]);// supprime le exit de la room : pas necessaire pour l instant
				newr.SupExit(room.exit[i]);
				return true;
			}
			i = (i + 1 < room.exit.Count) ? i + 1 : 0;
		}
		return false;
	}
	
	void SupExit(exit e)
	{
		int i = -1;

		while (++i < listporte.Count)
		{
			if (listporte[i] == e)
				listporte.RemoveAt(i);
		}
	}

	void RemoveExitInLp(exit e)
	{
		int i = -1;

		while (++i < listporte.Count)
		{
			if (listporte[i] == e)
				listporte.RemoveAt(i);
		}
	}

	void fillexit(Room r)
	{
		
	}
}

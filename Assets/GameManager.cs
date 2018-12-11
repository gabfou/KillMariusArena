using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameManager : MonoBehaviour {

	public GameSave save;
	// public int CheckpointPassed = 0;
	public PlayerController player;

	public static GameManager instance;
	public float DistanceOfSound = 50;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		// SaveLoad.Load();
		// instance = SaveLoad.savedGames.FirstOrDefault();
		// if (instance == null)
		// 	instance  = this;
		// lastCheckpoint = instance.lastCheckpoint;
		// CheckpointPassed = instance.CheckpointPassed;
		// Random.InitState(System.DateTime.Now.Millisecond);
		if (instance == null)
			instance = this;
		else 
			GameObject.Destroy(gameObject);
	}
}

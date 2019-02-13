using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public GameSave save;
	// public int CheckpointPassed = 0;
	public PlayerController player;

	public static GameManager instance;
	public float DistanceOfSound = 50;
	public Text life;
	[HideInInspector] public bool replacedPlayer = false;

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
		{
			replacedPlayer = false;
			GameObject.Destroy(gameObject);
		}
	}
}

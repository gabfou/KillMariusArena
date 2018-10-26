using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int lastCheckpoint = 0;
	public int CheckpointPassed = 0;
	public PlayerController player;

	public static GameManager instance;

	void Awake()
	{
		instance  = this;
	}
}

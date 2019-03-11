using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum Difficulty{Easy = 12, Normal = 8, Hard = 5, GoodLuck = 3};

public class GameManager : MonoBehaviour
{
	public GameSave save;
	public PlayerController player;
	public GamePref pref; // Pour l'intant y a que les control(genre 3 controle)

	public static GameManager instance;
	public float DistanceOfSound = 50;

	public Difficulty difficulty = Difficulty.Hard;
	public Text life;
	[HideInInspector] public bool replacedPlayer = false;
	[HideInInspector] public bool playerSpawned = false;
	[HideInInspector] public AudioSource audioSource;

	void OnEnable()
	{
		if (instance == null)
		{
			audioSource = GetComponent<AudioSource>();
			save.levelChangeReinit();
			instance = this;
		}
		else
		{		
			instance.replacedPlayer = false;
			instance.playerSpawned = false;
			GameObject.Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}


	public void setDifficulty(Difficulty diff)
	{
		difficulty = diff;
	}

	public void setDifficultyToEasy(){difficulty=Difficulty.Easy;}
	public void setDifficultyToNormal(){difficulty=Difficulty.Normal;}
	public void setDifficultyToHard(){difficulty=Difficulty.Hard;}
	public void setDifficultyToGoodLuck(){difficulty=Difficulty.GoodLuck;}
}

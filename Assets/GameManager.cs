using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum Difficulty{Easy = 10, Normal = 7, Hard = 5, GoodLuck = 3};

public class GameManager : MonoBehaviour
{
	public GameSave save;
	public PlayerController player;
	public GamePref pref;

	public static GameManager instance;
	public float DistanceOfSound = 50;

	public Difficulty difficulty = Difficulty.Hard;
	public Text life;
	[HideInInspector] public bool replacedPlayer = false;
	[HideInInspector] public bool playerSpawned = false;
	[HideInInspector] public AudioSource audioSource;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if (instance == null)
		{
			audioSource = GetComponent<AudioSource>();
			save.levelChangeReinit();
			instance = this;
		}
		else
		{
			if (instance.save.SceneName != SceneManager.GetActiveScene().name && SceneManager.GetActiveScene().name.Contains("level")) // pas safe lwgmnsbdfjhfhkklwq
				instance.save.levelChangeReinit();
			instance.replacedPlayer = false;
			instance.playerSpawned = false;
			GameObject.Destroy(gameObject);
		}
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

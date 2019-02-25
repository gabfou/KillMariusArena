using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	public GameSave save;
	public PlayerController player;

	public static GameManager instance;
	public float DistanceOfSound = 50;
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
			instance = this;
		}
		else
		{
			if (instance.save.SceneName != SceneManager.GetActiveScene().name)
				instance.save.levelChangeReinit();
			instance.replacedPlayer = false;
			instance.playerSpawned = false;
			GameObject.Destroy(gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public GameSave save;
	public PlayerController player;

	public static GameManager instance;
	public float DistanceOfSound = 50;
	public Text life;
	[HideInInspector] public bool replacedPlayer = false;
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
			instance.replacedPlayer = false;
			GameObject.Destroy(gameObject);
		}
	}
}

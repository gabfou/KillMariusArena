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
	public PrefabList playerPrefabList;
	[HideInInspector]public MedalManager medalManager;
	public Text life;
	[HideInInspector] public bool replacedPlayer = false;
	[HideInInspector] public bool playerSpawned = false;
	[HideInInspector] public AudioSource audioSource;
	[HideInInspector] public PathfinderGrid pathfinderGrid;

    IEnumerator ChangeMusicCoroutine(AudioClip intro, AudioClip music, float fadeTime, float volumeOfIntro, float volume)
    {
        float time = fadeTime;
        float volumeInitial = GameManager.instance.audioSource.volume;
		while (time > 0)
		{
			time -= Time.deltaTime;
			GameManager.instance.audioSource.volume = volumeInitial * (time / fadeTime);
			yield return new WaitForEndOfFrame();
		}
		if (intro)
		{
			GameManager.instance.audioSource.clip = intro;
			GameManager.instance.audioSource.volume = volumeOfIntro;
			GameManager.instance.audioSource.loop = false;
			GameManager.instance.audioSource.Play();
			yield return new WaitForSecondsRealtime(intro.length);
		}
		GameManager.instance.audioSource.clip = music;
		GameManager.instance.audioSource.volume = volume;
		GameManager.instance.audioSource.loop = true;
		GameManager.instance.audioSource.Play();
    }

	Coroutine ChangeMusicCurrent = null;
	public void ChangeMusic(AudioClip intro, AudioClip music, float fadeTime, float volumeOfIntro, float volume)
	{
		if ((audioSource.clip != music && audioSource.clip != intro) || audioSource.isPlaying == false)
		{
			if (ChangeMusicCurrent != null)
				StopCoroutine(ChangeMusicCurrent);
        	ChangeMusicCurrent = StartCoroutine(ChangeMusicCoroutine(intro, music, fadeTime, volumeOfIntro, volume));
		}
    } 

	void OnEnable()
	{
		if (instance == null)
		{
			audioSource = GetComponent<AudioSource>();
			medalManager = GetComponent<MedalManager>();
			GetComponent<io.newgrounds.core>().Init();
			pref.init();
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
		save.difficulty = diff;
	}

	public void setDifficultyToEasy(){save.difficulty=Difficulty.Easy;}
	public void setDifficultyToNormal(){save.difficulty=Difficulty.Normal;}
	public void setDifficultyToHard(){save.difficulty=Difficulty.Hard;}
	public void setDifficultyToGoodLuck(){save.difficulty=Difficulty.GoodLuck;}
}

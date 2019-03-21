using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicOnEnable : MonoBehaviour
{
    public AudioClip intro;
    [Range(0,1)]public float volumeOfIntro = 1;
    public AudioClip music;
    [Range(0,1)]public float volume = 1;
    public float timeToFadeInSec = 0.5f;
    public bool onlyFirtstTime = false;


    void OnEnable() {
        if (onlyFirtstTime == true)
        {
            float id = transform.position.sqrMagnitude;
            if (GameManager.instance.save.listOfObjectAlreadyUse.Contains(id))
		    	return ;
            GameManager.instance.save.listOfObjectAlreadyUse.Add(id);
        }
        GameManager.instance.ChangeMusic(intro, music, timeToFadeInSec, volumeOfIntro, volume);
    } 
}

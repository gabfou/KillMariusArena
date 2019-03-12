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


    void OnEnable() {
        GameManager.instance.ChangeMusic(intro, music, timeToFadeInSec, volumeOfIntro, volume);
    } 
}

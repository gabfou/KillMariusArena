using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicOnEnable : MonoBehaviour
{
    public AudioClip music;
    [Range(0,1)]public float volume = 1;

    void OnEnable() {
        if (GameManager.instance?.audioSource)
        {
            GameManager.instance.audioSource.clip = music;
            GameManager.instance.audioSource.volume = volume;
            GameManager.instance.audioSource.Play();
        }
        else
            Debug.LogWarning(name + " No GameManager.instance?.audioSource");
    } 
}

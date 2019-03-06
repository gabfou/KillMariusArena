using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicOnEnable : MonoBehaviour
{
    public AudioClip music;
    [Range(0,1)]public float volume = 1;
    public float timeToFadeInSec = 0.5f;


    IEnumerator ChangeMusic()
    {
        float time = timeToFadeInSec;
        float volumeInitial = GameManager.instance.audioSource.volume;
        while (time > 0)
        {
            time -= Time.deltaTime;
            GameManager.instance.audioSource.volume = volumeInitial * (time / timeToFadeInSec);
            yield return new WaitForEndOfFrame();
        }
        if (GameManager.instance?.audioSource)
        {
            if (GameManager.instance.audioSource.clip != music || GameManager.instance.audioSource.isPlaying == false)
            {
                GameManager.instance.audioSource.clip = music;
                GameManager.instance.audioSource.volume = volume;
                GameManager.instance.audioSource.Play();
            }
        }
        else
            Debug.LogWarning(name + " No GameManager.instance?.audioSource");
    }

    void OnEnable() {
        StartCoroutine(ChangeMusic());
    } 
}

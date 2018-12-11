using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActivateTimelineOnTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<PlayableDirector>().Play();
        GameObject.Destroy(this);
    }
}

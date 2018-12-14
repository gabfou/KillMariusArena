using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineDebile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(2.5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

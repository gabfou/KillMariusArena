using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineDebile : MonoBehaviour
{
    public float mult = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
       GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(mult);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

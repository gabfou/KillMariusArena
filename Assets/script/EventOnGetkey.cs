using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class EventOnGetkey : MonoBehaviour
{
    public List<KeyCode> key = new List<KeyCode>();
    public UnityEvent Event; 

    // Update is called once per frame
    void Update()
    {
        if (key.Any(k => Input.GetKeyDown(k)))
            Event.Invoke();
    }
}

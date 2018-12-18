using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventonEnable : MonoBehaviour
{
    public UnityEvent Event;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Event.Invoke();
    }
}

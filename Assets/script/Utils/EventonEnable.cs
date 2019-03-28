using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventonEnable : MonoBehaviour
{
    public UnityEvent Event;

    public float delay = -1;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        Event.Invoke();
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (delay > 0)
            StartCoroutine(Delay());
        else
            Event.Invoke();
    }
}

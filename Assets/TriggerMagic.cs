using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMagic : MonoBehaviour
{
    public UnityEvent   onTriggerEnter;
    public UnityEvent   onTriggerStay;
    public UnityEvent   onTriggerExit;
    // Start is called before the first frame update

    void OnTriggerExit2D(Collider2D other)
    {
        onTriggerExit.Invoke();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnter.Invoke();
    }

    private void OnTriggerStay2D(Collider2D other) {        
        onTriggerStay.Invoke();
    }

}

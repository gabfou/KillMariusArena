using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TriggerMagic : MonoBehaviour
{
    public string[] tagAccept = {"Player"};
    public UnityEvent   onTriggerEnter;
    public UnityEvent   onTriggerStay;
    public UnityEvent   onTriggerExit;
    public int nbOfUsage = -1;
    // Start is called before the first frame update

    void checkDestroy()
    {
        if (nbOfUsage > 0)
        {
            nbOfUsage--;
            if (nbOfUsage == 0)
                GameObject.Destroy(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (tagAccept.Any(c => c == other.tag))
        {
           onTriggerExit.Invoke();
            checkDestroy();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tagAccept.Any(c => c == other.tag))
        {
            onTriggerEnter.Invoke();
            checkDestroy();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (tagAccept.Any(c => c == other.tag))
        {
            onTriggerStay.Invoke();
            checkDestroy();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
 public class myCollider2DEvent : UnityEvent<Collider2D>{}

public class TriggerMagicV2 : MonoBehaviour
{
    public string[] tagAccept = {"Player"};
    public myCollider2DEvent   onTriggerEnter;
    public myCollider2DEvent  onTriggerStay;
    public myCollider2DEvent  onTriggerExit;
    // Start is called before the first frame update

    void OnTriggerExit2D(Collider2D other)
    {
        if (tagAccept.Any(c => c == other.tag))
           onTriggerExit.Invoke(other);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tagAccept.Any(c => c == other.tag))
            onTriggerEnter.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (tagAccept.Any(c => c == other.tag))
            onTriggerStay.Invoke(other);
    }

}

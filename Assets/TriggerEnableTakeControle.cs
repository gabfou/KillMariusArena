using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnableTakeControle : MonoBehaviour
{
    public string tagAccept = "Player";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == tagAccept || tagAccept == "")
        {
            takeControle tc = other.GetComponent<takeControle>();
            if (tc != null)
                tc.enabled = true;
        }
    }
}

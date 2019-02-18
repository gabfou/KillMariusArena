using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableOnEnable : MonoBehaviour
{

    public MonoBehaviour mono;

    IEnumerator lol()
    {
        yield return new WaitForSeconds(0.1f);
         mono.enabled = false;
    }
    void OnEnable()
    {
        
        // mono.enabled = false;
    }
    public void Vdsalidate()
    {
        StartCoroutine(lol());
    }

}

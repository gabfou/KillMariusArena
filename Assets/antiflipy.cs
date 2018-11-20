using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antiflipy : MonoBehaviour
{
    float starty;
    // Start is called before the first frame update
    void Start()
    {
        starty = transform.lossyScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Sign(transform.lossyScale.y) != Mathf.Sign(starty))
            transform.localScale = new Vector3(transform.localScale.x,-transform.localScale.y,transform.localScale.z);
        
    }
}

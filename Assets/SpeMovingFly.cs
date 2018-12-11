using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeMovingFly : MonoBehaviour
{
    Transform player = null;
    CaBouge caBouge;
    // Start is called before the first frame update
    void Start()
    {
        caBouge = GetComponent<CaBouge>();
    }

    void OnEnable()
    {
        player = GetComponentInChildren<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (caBouge.isDeplacing == false)
        {
            player.parent = null;
            GameObject.Destroy(gameObject);
        }
    }
}

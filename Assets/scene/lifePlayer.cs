using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lifePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.life = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

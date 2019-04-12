using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroyOnLoad : MonoBehaviour
{
    public static bool exist = false;
    void Start()
    {
        if (exist == false)
            DontDestroyOnLoad(gameObject);
        else
            GameObject.Destroy(gameObject);
    }
}

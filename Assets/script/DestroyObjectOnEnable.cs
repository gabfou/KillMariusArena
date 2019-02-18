using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOnEnable : MonoBehaviour
{
    public GameObject go;

    private void OnEnable()
    {
        GameObject.Destroy(go);
    }
}

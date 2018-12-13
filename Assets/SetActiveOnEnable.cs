using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnEnable : MonoBehaviour
{
    public GameObject obj;
    public bool value = true;

    private void OnEnable() {
        obj.SetActive(value);
    }

}

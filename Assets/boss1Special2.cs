using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss1Special2 : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<lever>().delayInSec = 1.5f;
    }

    // Update is called once per fram
}

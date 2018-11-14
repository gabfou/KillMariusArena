using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decaleDebile : MonoBehaviour
{
    Transform cible;
    // Start is called before the first frame update
    void Start()
    {
        cible = GameManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(cible.position.x - transform.position.x) > 2)
            transform.position = new Vector3(cible.position.x,transform.position.y,transform.position.z);
    }
}

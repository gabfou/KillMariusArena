using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRepeatOnChecpoint : MonoBehaviour
{
    float id;
    // Start is called before the first frame update
    void Start()
    {
        id = transform.position.sqrMagnitude;
        if (GameManager.instance.save.listOfObjectAlreadyUse.Contains(id))
            GameObject.Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.save.listOfObjectAlreadyUseButNotSave.Add(id);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRepeatOnChecpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float id = transform.position.sqrMagnitude;
        if (GameManager.instance.save.listOfObjectAlreadyUse.Contains(id))
            GameObject.Destroy(gameObject);
        else
            GameManager.instance.save.listOfObjectAlreadyUseButNotSave.Add(id);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerMedalNinja : MonoBehaviour
{
    float id = -1;
    void Start()
    {
        id = transform.position.sqrMagnitude;
        if (GameManager.instance.save.listOfObjectAlreadyUse.Contains(id))
            gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController pl = other.GetComponent<PlayerController>();
            if (pl.life == pl.maxLife)
                GameManager.instance.medalManager.TryToUnlockMedal("Ninja");
            if (!GameManager.instance.save.listOfObjectAlreadyUse.Contains(id))
                GameManager.instance.save.listOfObjectAlreadyUseButNotSave.Add(id);
            gameObject.SetActive(false);
        }
    }
}

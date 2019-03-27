using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerMedalNinja : MonoBehaviour
{
    int life = -1;
    void OnEnable()
    {
        life = GameManager.instance.player.life;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController pl = other.GetComponent<PlayerController>();
            if (pl.life == life)
                GameManager.instance.medalManager.TryToUnlockMedal("Ninja");
            gameObject.SetActive(false);
        }
    }
}

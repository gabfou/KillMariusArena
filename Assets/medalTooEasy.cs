using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class medalTooEasy : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManager.instance.save.difficulty == Difficulty.GoodLuck)
        {
            GameManager.instance.medalManager.TryToUnlockMedal("Too easy");
        }
    }

}

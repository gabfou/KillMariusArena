using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalEnd : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.instance.medalManager.TryToUnlockMedal("Victory");
        GameManager.instance.medalManager.TryToUnlockMedal("Easy");
        
        if (GameManager.instance.save.difficulty <= Difficulty.Normal)
            GameManager.instance.medalManager.TryToUnlockMedal("Normal");

        if (GameManager.instance.save.difficulty <= Difficulty.Hard)
            GameManager.instance.medalManager.TryToUnlockMedal("Hard");

        if (GameManager.instance.save.difficulty == Difficulty.GoodLuck)
            GameManager.instance.medalManager.TryToUnlockMedal("GoodLuck");

    }
}

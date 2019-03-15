using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalActivator : MonoBehaviour
{
    public float delayInSec = 0;


    IEnumerator ActivateMedalCoroutine(string medal)
    {
        yield return new WaitForSeconds(delayInSec);
        GameManager.instance.medalManager.TryToUnlockMedal(medal);
    }

    public void ActivateMedal(string medal)
    {
        StartCoroutine(ActivateMedalCoroutine(medal));
    }
}

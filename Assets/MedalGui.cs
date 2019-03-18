using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalGui : MonoBehaviour
{
    [HideInInspector] Text text;
    [HideInInspector] Animator animator;
    float animTime = 5;
    bool isInMedalBeingActivated = false;
    void Start()
    {
        GameManager.instance.medalManager.medalGui = this;
        text = GetComponentInChildren<Text>();
        animator = GetComponent<Animator>();
    }

    IEnumerator MedalBeingActivated()
    {
        isInMedalBeingActivated = true;
        text.text = name;
        yield return new WaitForSeconds(animTime);
        isInMedalBeingActivated = false;
    }

    IEnumerator WaitForMedalBeingActivated()
    {
        while (isInMedalBeingActivated)
            yield return null;
        StartCoroutine(MedalBeingActivated());
    }

    public void ActivateMedal(string name, Sprite sprite)
    {
        text.text = name;
        StartCoroutine(WaitForMedalBeingActivated());
    }

}

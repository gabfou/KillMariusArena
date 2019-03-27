using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalGui : MonoBehaviour
{
    [HideInInspector] Text text;
    [HideInInspector] Animator animator;
    Image image;
    float animTime = 5;
    bool isInMedalBeingActivated = false;
    void Start()
    {
        GameManager.instance.medalManager.medalGui = this;
        text = GetComponentInChildren<Text>();
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    IEnumerator MedalBeingActivated(string name, Sprite sprite)
    {
        text.text = name;
        image.sprite = sprite;
        isInMedalBeingActivated = true;
        animator.SetTrigger("trigger");
        yield return new WaitForSeconds(animTime);
        isInMedalBeingActivated = false;
    }

    IEnumerator WaitForMedalBeingActivated(string name, Sprite sprite)
    {
        while (isInMedalBeingActivated)
            yield return null;
        StartCoroutine(MedalBeingActivated(name, sprite));
    }

    public void ActivateMedal(string name, Sprite sprite)
    {

        StartCoroutine(WaitForMedalBeingActivated(name, sprite));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuiteOfTextOnEnable : MonoBehaviour {

    public List<string>    list;
    Text                   t;
    public float TimeBetweenText = 3f;

    void OnValidate()
    {
        TimeBetweenText = Mathf.Max(TimeBetweenText, 0.5f);
    }

    IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
       // i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return new WaitForSeconds(t / 100);
        }
    }

    IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        //i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator LanceTout()
    {
        int i = -1;
        while (++i < list.Count)
        {
            t.text = list[i];
            StartCoroutine(FadeTextToFullAlpha(0.4f, t));
            yield return new WaitForSeconds(TimeBetweenText - 0.5f);
            StartCoroutine(FadeTextToZeroAlpha(0.4f, t));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnEnable()
    {
        t = GetComponent<Text>();
        if (list == null)
        {
            Debug.Log("SuiteOfTextOnEnableFail List null 5468754867");
            return;
        }
        StartCoroutine(LanceTout());
    }

}

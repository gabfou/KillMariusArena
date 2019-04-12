using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeControl : MonoBehaviour
{
    public string paramName;
    KeyCode key;
    Text text;
    public ButtonList listOfButtonToDeactivate;
    public GameObject KeyPressPanel;

    // Start is called before the first frame update
    void Start()
    {
        key = (KeyCode)GameManager.instance.pref.GetType().GetProperty(paramName).GetValue(GameManager.instance.pref);
        text = GetComponentInChildren<Text>();
        text.text = key.ToString();
    }

    IEnumerator changeKeyEnum()
    {
        KeyPressPanel.SetActive(true);
        listOfButtonToDeactivate.list.ForEach(b => b.interactable = false);
        while (true)
        {
            if (Input.anyKey)
            {
                foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if(Input.GetKey(vKey))
                    {
                        key = vKey;
                        break ;
                    }
                }
                break ;
            }
            yield return new WaitForEndOfFrame();
        }
        GameManager.instance.pref.GetType().GetProperty(paramName).SetValue(GameManager.instance.pref, key);
        text.text = key.ToString();
        listOfButtonToDeactivate.list.ForEach(b => b.interactable = true);
        KeyPressPanel.SetActive(false);
    }
    public void changeKey()
    {
        StartCoroutine(changeKeyEnum());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        listOfButtonToDeactivate.list.ForEach(b => b.interactable = true);
        KeyPressPanel.SetActive(false);
    }
}

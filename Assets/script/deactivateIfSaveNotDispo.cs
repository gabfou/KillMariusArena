using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deactivateIfSaveNotDispo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("save") == null || PlayerPrefs.GetString("save")  == "")
            GetComponent<Button>().interactable = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ButtonList : MonoBehaviour
{
    public bool addAllChild = true;
    public List<Button> list = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        if (addAllChild)
            list.AddRange(GetComponentsInChildren<Button>());
    }
}

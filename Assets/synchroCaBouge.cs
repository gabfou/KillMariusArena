using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class synchroCaBouge : MonoBehaviour
{
    public List<CaBouge> list;
    List<int> listE = new List<int>();
    List<bool> listb = new List<bool>();
    void Start()
    {
        foreach(var cb in list)
        {
            listE.Add(cb.e);
            listb.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach(var cb in list)
        {
            if (cb.e != listE[i])
            {
                listb[i] = true;
                listE[i] = cb.e;
                cb.isDeplacing = false;
            }
            i++;
        }
        if (listb.All(b => b == true))
        {
            list.ForEach(cb => cb.isDeplacing = true);
            for(int j = 0; j < listb.Count; j++)
                listb[j] = false;
        }
    }
}

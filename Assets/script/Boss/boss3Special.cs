using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class boss3Special : MonoBehaviour
{
    public Agro[] chauveSouris;
    public UnityEvent End;
    float initLife = 0;
    public Slider Life;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Agro a in chauveSouris)
        {
            if (a != null)
                initLife += a.life;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Life.enabled == false)
            return ;
        float curentlife = 0;
        foreach(Agro a in chauveSouris)
        {
            if (a != null)
                curentlife += a.life;
        }
        Life.value = curentlife / initLife;
        if (chauveSouris.All(a => a == null))
        {
            End.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class boss3Special : MonoBehaviour
{
    public Agro[] chauveSouris;
    public UnityEvent End;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (chauveSouris.All(a => a == null))
        {
            End.Invoke();
        }
    }
}

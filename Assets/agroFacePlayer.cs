using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agro))]
public class agroFacePlayer : MonoBehaviour
{
    Agro agro;
    void Start()
    {
        agro = GetComponent<Agro>();
    }

    void Update()
    {
        if (agro.Cible != null)
            agro.FacePlayer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// cette class ne select rien elle s'occupe juste de garder les button de la bonne couleur (le select ce fait dans les event des button)

public class DifficultySelect : MonoBehaviour
{
    public Text[] difficultyInInvOrder;

    public void ChangeColor()
    {
        foreach(var t in difficultyInInvOrder)
            t.color = Color.black;
        difficultyInInvOrder[Array.IndexOf(Enum.GetValues(GameManager.instance.difficulty.GetType()), GameManager.instance.difficulty)].color = Color.red; // rend le text a l'index de la difficulte dans l'enum red
    }

    void Update()
    {
        ChangeColor();
    }
}

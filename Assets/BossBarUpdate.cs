using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBarUpdate : MonoBehaviour
{
    Agro agro;
    public Slider slider;
    float MaxHp;
    // Start is called before the first frame update
    void Start()
    {
        agro = GetComponent<Agro>();
        MaxHp = agro.life;
    }

    // Update is called once per frame
    void Update()
    {
        if (agro.Cible)
        {
            slider.gameObject.SetActive(true);
            slider.value = agro.life / MaxHp;
        }

    }
}

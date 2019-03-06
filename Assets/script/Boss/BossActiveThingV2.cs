using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossActiveThingV2 : MonoBehaviour
{
    public int lifeTreshold;
    public UnityEvent eventToTrigger;

    Character character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (character.life > lifeTreshold)
            return ;
        eventToTrigger.Invoke();
        GameObject.Destroy(this);
    }
}

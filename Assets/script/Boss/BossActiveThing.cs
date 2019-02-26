using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActiveThing : MonoBehaviour
{
    public int lifeTreshold;
    public List<GameObject> thingToActive;

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
        thingToActive.ForEach(t => t.SetActive(true));
        GameObject.Destroy(this);
    }
}

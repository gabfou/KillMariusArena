using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActiveThing : MonoBehaviour
{
    public int lifeTreshold;
    public List<GameObject> thingToActive;

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.life > lifeTreshold)
            return ;
        thingToActive.ForEach(t => t.SetActive(true));
        GameObject.Destroy(this);
    }
}

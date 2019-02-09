using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Fatality : MonoBehaviour
{
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
        player.cannotmove = true;
        player.canControle = false;
    }

    Vector2 r = Vector2.zero;
    // Update is called once per frame
    void Update()
    {
        Vector2 target = transform.position + (player.transform.position - transform.position).normalized * 2;
        player.transform.position = Vector2.SmoothDamp(player.transform.position, target, ref r, 1);
    }
}

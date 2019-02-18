using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeControle : MonoBehaviour
{
    public float move;
    public float movey;
    PlayerController player;
    // Start is called before the first frame update
    private void OnEnable() {
        player = GameManager.instance.player;
        player.canControle = false;
    }

    private void Update() {
        player.move = move;
        player.movey = movey;
    }

    private void OnDisable() {
        player.canControle = true;
    }
}

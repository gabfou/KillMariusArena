using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyOnCollide : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        GameObject.Destroy(gameObject);
    }
}

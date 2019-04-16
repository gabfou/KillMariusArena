using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuage : MonoBehaviour
{
    Transform player;
    public float speedMin = 5;
    public float speedMax = 15;
    float speed;
    float startDiff;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        player = GameManager.instance.player.transform;
        speed = Random.Range(speedMin, speedMax);
        startDiff = Mathf.Abs(transform.position.x - player.position.x) * 1.2f;
    }
    // Update is called once per frame
    void Update()
    {
        if (startDiff < Mathf.Abs(transform.position.x - player.position.x))
            GameObject.Destroy(gameObject);
        else
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}

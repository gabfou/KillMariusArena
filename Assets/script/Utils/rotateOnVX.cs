using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateOnVX : MonoBehaviour {

    Rigidbody2D rb;
    public float mult = 5;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, -mult * Time.deltaTime * rb.velocity.x));
	}
}

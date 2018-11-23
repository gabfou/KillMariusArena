using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCorectVelocity : MonoBehaviour {

	Transform cible;
	Rigidbody2D rb;

	public float rotationspeed = 2;
	// Use this for initialization
	void Start () {
		cible = GameManager.instance.player.transform;
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
				int sideToTurnTo = 0;
       
        Vector3 angleToSpotRelativeToMe = cible.position - transform.position;
       
        // get its cross product, which is the axis of rotation to
        // get from one vector to the other
        Vector3 cross = Vector3.Cross(rb.velocity, angleToSpotRelativeToMe);
 
        if (cross.z < 0) {
            sideToTurnTo = -1;
        }
        else {
            sideToTurnTo = 1;
        }
		rb.velocity = Quaternion.Euler(0, 0, sideToTurnTo * rotationspeed) * rb.velocity;
	}
}

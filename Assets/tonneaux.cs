using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tonneaux : MonoBehaviour {

	public float speed;
	public int side;
	Rigidbody2D rbody;
	List<int> l = new List<int>();
	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rbody.velocity = new Vector2( side * speed, rbody.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (l.Contains(other.gameObject.GetInstanceID()))
			return ;
		l.Add(other.gameObject.GetInstanceID());
		if (other.gameObject.tag == "ChangeDirTonneaux")
			side  = -side;
		else if (other.gameObject.tag == "DestroyTonneaux")
			GameObject.Destroy(gameObject);	
	}
}

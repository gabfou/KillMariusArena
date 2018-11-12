using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaBouge : MonoBehaviour {

	public bool looping = true;
	public bool activeOnAwake = true;
	public List<Vector2> listOfPassage = new List<Vector2>();
	public bool isDeplacing = false;
	public float speed;
	Rigidbody2D rigidbody;
	List<Transform> l = new List<Transform>();

	public int e = 0; 
	float marge;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		listOfPassage.Insert(0, transform.position);
		if (activeOnAwake)
			isDeplacing = true;
		marge = 0.1f;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (isDeplacing)
		{
			rigidbody.velocity = (listOfPassage[e] - (Vector2)transform.position).normalized * Time.fixedDeltaTime * speed;
			if (Vector2.Distance(transform.position, listOfPassage[e]) < marge)
			{
				if (listOfPassage.Count <= ++e)
				{
					e = 0;
					if (looping == false)
					{
						rigidbody.velocity = Vector2.zero;
						isDeplacing = false;
					}
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		// Debug.Log(other.gameObject.name);
		PlayerController player;
		if ((player = other.gameObject.GetComponent<PlayerController>()) && other.transform.parent == null && other.gameObject.GetComponent<FlyingAgro>() == null)
		{
			other.transform.parent = transform;
			player.rbparent = rigidbody;
			l.Add(other.transform);
		}
	}

	IEnumerator WaitBeforeDesync(Transform t)
	{
		yield return new WaitForSeconds(0.1f);
		t.parent = null;
		t.GetComponent<PlayerController>().rbparent = null;
		l.Remove(t);
	}

	void OnCollisionExit2D(Collision2D other)
	{
		Transform t = l.Find(c => c.GetInstanceID() == other.transform.GetInstanceID());
		if (t)
		{
			t.parent = null;
			t.GetComponent<PlayerController>().rbparent = null;
			l.Remove(t);
		}
	}
}

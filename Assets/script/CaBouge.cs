using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CaBouge : MonoBehaviour {

	public bool looping = true;
	public bool activeOnAwake = true;
	public List<Vector2> listOfPassage = new List<Vector2>();
	public bool isDeplacing = false;
	public float speed;
	Rigidbody2D rigidbody;
	List<Transform> l = new List<Transform>();
	public List<Rigidbody2D> AffectAlso = new List<Rigidbody2D>();

	public int e = 0; 
	
	[Min(0.1f)]
	public float marge = 0.1f;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		listOfPassage.Insert(0, transform.position);
		if (activeOnAwake)
			isDeplacing = true;
		if (marge < 0.1f)
			marge = 0.1f;
	}
	
	void FixedUpdate ()
	{
		if (!isDeplacing)
			return;

		// deplacement
		if (rigidbody)
		{
			rigidbody.velocity = (listOfPassage[e] - (Vector2)transform.position).normalized * Time.fixedDeltaTime * speed;
			AffectAlso.ForEach(aa => {if (aa) aa.velocity = rigidbody.velocity;});
		}
		else
			transform.position += (Vector3)((listOfPassage[e] - (Vector2)transform.position).normalized * Time.fixedDeltaTime * speed);


		// next Point
		if (Vector2.Distance(transform.position, listOfPassage[e]) < marge)
		{
			if (listOfPassage.Count <= ++e)
			{
				e = 0;
				if (looping == false)
				{
					if (rigidbody)
						rigidbody.velocity = Vector2.zero;
					isDeplacing = false;
					AffectAlso.ForEach(aa => aa.velocity = Vector2.zero);
				}
			}
		}


	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Character player;
		if (rigidbody && (player = other.gameObject.GetComponent<Character>()) && other.transform.parent == null && other.gameObject.GetComponent<FlyingAgro>() == null)
		{
			other.transform.parent = transform;
			player.rbparent = rigidbody;
			l.Add(other.transform);
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		Transform t = l.Find(c => c.GetInstanceID() == other.transform.GetInstanceID());
		if (t)
		{
			t.parent = null;
			t.GetComponent<Character>().rbparent = null;
			l.Remove(t);
		}
	}
}

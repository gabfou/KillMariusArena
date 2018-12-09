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

	[HideInInspector] public int e = 0; 
	
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
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (isDeplacing && rigidbody)
		{
			rigidbody.velocity = (listOfPassage[e] - (Vector2)transform.position).normalized * Time.fixedDeltaTime * speed;
			AffectAlso.ForEach(aa => {if (aa) aa.velocity = (listOfPassage[e] - (Vector2)transform.position).normalized * Time.fixedDeltaTime * speed;});
			if (Vector2.Distance(transform.position, listOfPassage[e]) < marge)
			{
				if (listOfPassage.Count <= ++e)
				{
					e = 0;
					if (looping == false)
					{
						rigidbody.velocity = Vector2.zero;
						isDeplacing = false;
						AffectAlso.ForEach(aa => aa.velocity = Vector2.zero);
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

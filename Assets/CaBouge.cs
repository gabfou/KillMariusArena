using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaBouge : MonoBehaviour {

	public bool looping = true;
	public bool activeOnAwake = true;
	public List<Vector3> listOfPassage = new List<Vector3>();
	[HideInInspector]public bool isDeplacing = false;
	public float speed;
	List<Transform> l;

	public int e = 0; 
	float marge;

	// Use this for initialization
	void Start () {
		listOfPassage.Add(transform.position);
		if (activeOnAwake)
			isDeplacing = true;
		marge = speed * 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isDeplacing)
		{
			transform.position = Vector3.MoveTowards(transform.position, listOfPassage[e], Time.deltaTime * speed);
			l.ForEach(c => c.transform.position = Vector3.MoveTowards(c.transform.position, listOfPassage[e], Time.deltaTime * speed));
			if (Vector3.Distance(transform.position, listOfPassage[e]) < marge)
			{
				if (listOfPassage.Count >= ++e)
					e = 0;
				if (looping == false)
					isDeplacing = false;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<PlayerController>())
			l.Add(other.transform);
	}

	void OnCollisionExit2D(Collision2D other)
	{
		l.Remove(l.Find(c => c.GetHashCode() == other.GetHashCode()));
	}
}

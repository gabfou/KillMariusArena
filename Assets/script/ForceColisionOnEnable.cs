using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceColisionOnEnable : MonoBehaviour {

	// Use this for initialization

	Collider2D col;

	private void Start()
	{
		col = GetComponent<Collider2D>();
	}

	private void OnEnable()
	{
		Collider2D[] result = new Collider2D[100];
		ContactFilter2D cf = new ContactFilter2D();
		cf.NoFilter();

		int size = Physics2D.OverlapCollider(col, cf, result);
		int i = -1;
		while (++i < size)
			col.gameObject.SendMessage("OnTriggerEnter", result[i]);
	}

}

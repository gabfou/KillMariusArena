using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChefAgro : MonoBehaviour {

	Agro agro;
	bool done = false;
	public List<Agro> Subordonate;
	// Use this for initialization
	void Start () {
		agro = GetComponent<Agro>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!done && agro && agro.Cible)
		{
			Subordonate.ForEach(s => s.Cible = agro.Cible);
			done = true;
		}

			
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// use a trigger do defien when this work;
// public class NuageBaladeur : MonoBehaviour {


// 	public List<GameObject> sprite;
// 	public float EcartMoyenEntreNuageInSecond = 0.2f;
// 	public float VitesseMoyenneEntreNuageInSecond = 0.2f;

// 	public float 
// 	public bool stayoncamerax = true;

// 	bool active = false;


// 	List<GameObject> intanciated; 


// 	public Bounds nuageZone;


// 	IEnumerator spawnNuage()
// 	{
// 		while (true)
// 		{

// 			yield return new WaitForSeconds(0.2f);
// 		}
// 	}
// 	// Use this for initialization
// 	void Start () {
// 		StartCoroutine("spawnNuage");

// 	}
	
// 	// Update is called once per frame
// 	void Update () {
// 		if (stayoncamerax == true)
// 			transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
// 		foreach (GameObject o in intanciated)
// 		{
// 			if (o != null)
// 			{
// 				if (o.transform.position.y)
// 			}
// 		}
// 	}

// 	private void OnTriggerEnter2D(Collider2D other)
// 	{
// 		if (other.tag == "Player")
// 			active = true;
// 	}


// 	private void OnTriggerExit2D(Collider2D other)
// 	{
// 		if (other.tag == "Player")
// 			active = false;
// 	}

// }
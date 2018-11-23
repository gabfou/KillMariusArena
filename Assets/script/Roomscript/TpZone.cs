using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TpZone : MonoBehaviour {

	public Vector3 exit;
	public Room exitroom;
	Collider2D zonetp;

	void Start () {
		if (!GetComponent<Collider2D>())
		{
			zonetp = gameObject.AddComponent<Collider2D>();
			zonetp.isTrigger = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.gameObject.transform.position = exit;
			CinemachineConfiner confiner = (Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera).GetComponent<CinemachineConfiner>();
			confiner.m_BoundingShape2D = exitroom.Colliderbounds;
			confiner.InvalidatePathCache();
		}
	}
}

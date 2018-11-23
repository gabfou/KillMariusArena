using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider2D))]
public class TriggerChangeTargetgroupofPlayerSize : MonoBehaviour {

	public float newsSize = 12;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!other || other.tag != "Player" || other.GetComponent<CinemachineTargetGroup>() == null)
			return ;
		if (other.GetComponent<CinemachineTargetGroup>().IsEmpty == false)
			other.GetComponent<CinemachineTargetGroup>().m_Targets[0].radius = newsSize;
	}
}

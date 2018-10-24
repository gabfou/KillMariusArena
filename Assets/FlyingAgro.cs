using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAgro : Agro
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void FixedUpdate ()
	{
		move = 1;
		movey = 1;
		PCFixedUpdate();
	}

	override protected void GroundCheck()
	{
		IsOnLadder = true;
	}
}

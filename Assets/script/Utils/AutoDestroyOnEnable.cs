using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		GameObject.Destroy(gameObject);
	}	
}

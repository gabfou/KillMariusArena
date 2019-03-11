using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    float oldTimeScale = 1;
	GameObject	Child;

    void Start() {
		Child = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
			{
                Time.timeScale = oldTimeScale;
				Child.SetActive(false);
			}
            else
            {
                oldTimeScale = Time.timeScale;
                Time.timeScale = 0;
				Child.SetActive(true);
            }
        }
    }
}

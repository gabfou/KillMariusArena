using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    float oldTimeScale = 1;
	Image	image;

    void Start() {
		image = GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
			{
                Time.timeScale = oldTimeScale;
				image.enabled = false;
			}
            else
            {
                oldTimeScale = Time.timeScale;
                Time.timeScale = 0;
				image.enabled = true;
            }
        }
    }
}

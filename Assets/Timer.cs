using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour
{
    public float timeInSec = 60;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeInSec -= Time.deltaTime;
        
        System.TimeSpan t = System.TimeSpan.FromSeconds(timeInSec);
        text.text = string.Format("{0:00}:{1:000}", t.Seconds, t.Milliseconds);

        if (timeInSec < 0)
            GameObject.Destroy(gameObject);
        

    }
}

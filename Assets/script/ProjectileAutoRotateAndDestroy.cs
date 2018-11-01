using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectileAutoRotateAndDestroy : MonoBehaviour {

    public float timeBeforeDestroy = 1;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public IEnumerator WaitTilDestroy()
    {
        float time = 0;
        Collider2D[] collist = GetComponents<Collider2D>();

        foreach(Collider2D g in collist)
            GameObject.Destroy(g);
        while (time < timeBeforeDestroy)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        Destroy(gameObject);
    }

    bool beginToDestroy = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (beginToDestroy)
            return ;
        beginToDestroy = true;
        GetComponent<Rigidbody2D>().AddTorque(10, ForceMode2D.Impulse);
        StartCoroutine(WaitTilDestroy());
    }
}

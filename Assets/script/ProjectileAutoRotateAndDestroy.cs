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
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color c = spriteRenderer.color;
        float s = transform.localScale.x;
        while (time < timeBeforeDestroy)
        {
            yield return new WaitForEndOfFrame();
            c.a -= Time.deltaTime / timeBeforeDestroy;
            spriteRenderer.color = c;
            transform.localScale -= new Vector3(Time.deltaTime / timeBeforeDestroy * s,Time.deltaTime / timeBeforeDestroy * s);
            time += Time.deltaTime;
        }
        Destroy(gameObject);
    }

    bool beginToDestroy = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (beginToDestroy)
            return ;
        Collider2D[] collist = GetComponents<Collider2D>();

        for (int i = 0; i < collist.Length; i++)
            GameObject.Destroy(collist[i]);
        beginToDestroy = true;
        GetComponent<Rigidbody2D>().AddTorque(15 * Mathf.Sign(GetComponent<Rigidbody2D>().rotation), ForceMode2D.Impulse);
        StartCoroutine(WaitTilDestroy());
    }
}

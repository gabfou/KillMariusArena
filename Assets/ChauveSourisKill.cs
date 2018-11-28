using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChauveSourisKill : MonoBehaviour
{
    public GameObject ReplaceBy;
    public float TimeBeforeReplace = 1;
    bool HasDo = false;


    IEnumerator Replace()
    {
        yield return new WaitForSeconds(TimeBeforeReplace);
        GameObject.Instantiate(ReplaceBy, transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!HasDo && other.tag == "KillChauveSouris")
        {
            HasDo = true;
            StartCoroutine(Replace());
        }

    }
}

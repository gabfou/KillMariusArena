using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public List<GameObject> clouds;
    public float timeBetweenNuage = 0.2f;
    public float timeRandome = 3;

    IEnumerator NuageSpawner()
    {
        while(true)
        {
            yield return new WaitForSeconds(timeBetweenNuage + Random.Range(0f, timeRandome));
            GameObject.Instantiate(clouds[Random.Range(0, clouds.Count)]);
        }
    }

    private void Start() {
        if (clouds.Count > 0)
            StartCoroutine(NuageSpawner());
        else
            Debug.Log("mo clouds in cloudmanager name: " + name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PrefabList", menuName = "ScriptableObjects/prefabList", order = 1)]
public class PrefabList : ScriptableObject
{
    public GameObject[] list;
}

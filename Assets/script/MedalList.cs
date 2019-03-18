using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "listMedal", menuName = "ScriptableObjects/listMedal", order = 1)]
public class MedalList : ScriptableObject
{
    [System.Serializable]
    public class Medal
    {
        public string name;
        public int id;
        public Sprite sprite;
    } 

    public List<Medal> list;
    public int dafuq;

    //     Dictionary<string, int> medalToId = new Dictionary<string, int>()
    // {
    //     {"Kidnaping!", 56710},
    //     {"BAM", 56711},
    //     {"BOOM!!!", 56712},
    //     {"BLAM!!!!!!", 56713},
    //     {"First Boss", 56714},
    //     {"Ninja", 56715},
    //     {"Boss2", 56716},
    // };

}

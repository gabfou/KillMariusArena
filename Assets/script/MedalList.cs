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
        public bool unlocked = false;

        public Medal(string name, int id, Sprite sprite = null)
        {
            this.name = name;
            this.id = id;
            this.sprite = sprite;
        }
    } 

    public Medal[] list = 
    {
        new Medal("Kidnaping!", 56710),
        new Medal("BAM", 56711),
        new Medal("BOOM!!!", 56712),
        new Medal("BLAM!!!!!!", 56713),
        new Medal("First Boss", 56714),
        new Medal("Ninja", 56715),
        new Medal("Boss2", 56716),
        new Medal("Too easy", 56750),
        new Medal("Boss3", 56751),
        new Medal("Boss4", 56752),
        new Medal("Easy", 56754),
        new Medal("Normal", 56755),
        new Medal("Hard", 56756),
        new Medal("GoodLuck", 56757),
        new Medal("Victory", 56753),
    };
}

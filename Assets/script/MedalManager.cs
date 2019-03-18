using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalManager : MonoBehaviour
{
// #if NEWGROUND 
    io.newgrounds.core ngio_core;
    [HideInInspector] public MedalGui medalGui;

    Dictionary<string, int> medalToId = new Dictionary<string, int>()
    {
        {"Kidnaping!", 56710},
        {"BAM", 56711},
        {"BOOM!!!", 56712},
        {"BLAM!!!!!!", 56713},
        {"First Boss", 56714},
        {"Ninja", 56715},
        {"Boss2", 56716},
        {"Too easy", 56750},
        {"Boss3", 56751},
        {"Boss4", 56752},
        {"Easy", 56754},
        {"Normal", 56755},
        {"Hard", 56756},
        {"GoodLuck", 56757},
        {"Victory", 56753},
    };


    private void Start() {
        ngio_core = GetComponent<io.newgrounds.core>();
    }

        // this will get called whenever a medal gets unlocked via unlockMedal()
    void onMedalUnlocked(io.newgrounds.results.Medal.unlock result) {
        io.newgrounds.objects.medal medal = result.medal;
        Debug.Log( "Medal Unlocked?: " + result.success + " name: " + medal.name + " (" + medal.value + " points)" );
    }

    // call this method whenever you want to unlock a medal.
    void unlockMedal(int medal_id) {
        // create the component
        io.newgrounds.components.Medal.unlock medal_unlock = new io.newgrounds.components.Medal.unlock();

        // set required parameters
        medal_unlock.id = medal_id;

        // call the component on the server, and tell it to fire onMedalUnlocked() when it's done.
        medal_unlock.callWith(ngio_core, onMedalUnlocked);
    }
// #endif

    public void TryToUnlockMedal(string medal)
    {
        unlockMedal(medalToId[medal]);
    }
}
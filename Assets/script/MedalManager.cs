using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MedalManager : MonoBehaviour
{
// #if NEWGROUND 
    io.newgrounds.core ngio_core;
    [HideInInspector] public MedalGui medalGui;

    public MedalList medalList;


    private void Start() {
        ngio_core = GetComponent<io.newgrounds.core>();
        if (ngio_core == null)
            Debug.Log("WTF2");
    }

        // this will get called whenever a medal gets unlocked via unlockMedal()
    void onMedalUnlocked(io.newgrounds.results.Medal.unlock result) {
        io.newgrounds.objects.medal medal = result.medal;
        Debug.Log( "Medal Unlocked?: " + result.success + " name: " + medal.name + " (" + medal.value + " points)" );
        if (result.success)
            GameManager.instance.medalManager.medalGui.ActivateMedal(medal.name, medalList.list.FirstOrDefault(m => m.id == medal.id).sprite);
    }

    // call this method whenever you want to unlock a medal.
    void unlockMedal(int medal_id) {
        if (ngio_core == null)
            Debug.Log("WTF");
        // create the component
        io.newgrounds.components.Medal.unlock medal_unlock = new io.newgrounds.components.Medal.unlock();

        // set required parameters
        medal_unlock.id = medal_id;

        // call the component on the server, and tell it to fire onMedalUnlocked() when it's done.
        medal_unlock.callWith(ngio_core, onMedalUnlocked);
    }
// #endif

    public void TryToUnlockMedal(string medalName)
    {
        MedalList.Medal medal = medalList.list.FirstOrDefault(m => m.name == medalName);
        if (medal != null)
            unlockMedal(medal.id);
        else
            Debug.LogWarning("medal: " + medalName + " not found");
    }

    private void Update() {
        if (ngio_core == null)
            Debug.Log(name + "WTF3");
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaEffectParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// OnParticleTrigger is called when any particles in a particle system
    /// meet the conditions in the trigger module.
    /// </summary>
    void OnParticleTrigger()
    {
        Debug.Log("fdsf");
    }
}

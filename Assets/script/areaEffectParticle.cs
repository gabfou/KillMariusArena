using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaEffectParticle : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        if (!ps)
            return ;
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        for (int i = 0; i < numInside; i++)
        {
            ParticleSystem.Particle p = inside[i];
            p.velocity += new Vector3(-1,0,0);
            p.velocity = new Vector3(Mathf.Clamp(p.velocity.x, -20, 20), p.velocity.y, p.velocity.z);
            inside[i] = p;
        }

        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = exit[i];
            p.velocity = new Vector3(0, p.velocity.y, p.velocity.z);
            exit[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        // Debug.Log("numInside " + numInside);
    }
}

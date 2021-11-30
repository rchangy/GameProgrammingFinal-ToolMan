using UnityEngine;

public class Explosion : Effect
{
    public override void PlayEffect()
    {
        ParticleSystem[] partList = transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem child in partList) {
            child.Play();
        }
    }

}

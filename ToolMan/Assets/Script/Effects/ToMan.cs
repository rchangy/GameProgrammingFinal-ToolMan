using UnityEngine;

public class ToMan : Effect
{
    // It should be triggered by PlayerController

    public override void PlayEffect() {
        effect.GetComponent<ParticleSystem>().Play();
    }
}

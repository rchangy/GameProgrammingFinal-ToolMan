using UnityEngine;

public class ToMan : Effect
{
    public override void PlayEffect() {
        effect.GetComponent<ParticleSystem>().Play();
    }

}

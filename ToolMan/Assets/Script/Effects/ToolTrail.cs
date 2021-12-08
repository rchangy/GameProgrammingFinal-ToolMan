using UnityEngine;

public class ToolTrail : Effect
{
    public override void PlayEffect() {
        effect.GetComponent<ParticleSystem>().Play();
    }

    public override void StopEffect()
    {
        effect.GetComponent<ParticleSystem>().Stop();
    }

}

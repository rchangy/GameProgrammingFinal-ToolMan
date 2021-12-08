using UnityEngine;

public class WhaleBigSkillEffect : Effect
{
    public override void PlayEffect() {
        effect.GetComponent<ParticleSystem>().Play();
    }

    public override void StopEffect()
    {
        effect.GetComponent<ParticleSystem>().Stop();
    }

}

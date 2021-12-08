using UnityEngine;

public class WhaleBigSkillWarning : Effect
{
    public override void PlayEffect() {
        effect.GetComponent<ParticleSystem>().Play();
    }

}

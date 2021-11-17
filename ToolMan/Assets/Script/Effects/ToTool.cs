using UnityEngine;

public class ToTool : Effect
{
    public override void PlayEffect() {
        effect.GetComponent<ParticleSystem>().Play();
    }

}

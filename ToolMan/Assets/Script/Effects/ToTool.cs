using UnityEngine;

public class ToTool : Effect
{
    // It should be triggered by PlayerController

    public override void PlayEffect() {
        effect.GetComponent<ParticleSystem>().Play();
    }

}

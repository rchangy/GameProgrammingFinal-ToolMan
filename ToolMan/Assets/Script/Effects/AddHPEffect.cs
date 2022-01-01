using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHPEffect : Effect
{
    public override void PlayEffect()
    {
        gameObject.transform.Find("HP").GetComponent<ParticleSystem>().Play();
        gameObject.transform.Find("ground").GetComponent<ParticleSystem>().Play();
    }
}

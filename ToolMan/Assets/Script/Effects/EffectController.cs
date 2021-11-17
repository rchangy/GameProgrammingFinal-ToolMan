using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] private Transform baseTrans; // Transform of Player or Enemy, etc.
    [SerializeField] private List<Effect> effectList = new List<Effect>();

    private void Awake()
    {
        // Set baseTrans of effects
        foreach (Effect e in effectList)
        {
            e.setBaseTransform(baseTrans);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            effectList[0].PlayEffect();
            //effectList[0].GetComponent<ParticleSystem>().Play();
    }
}

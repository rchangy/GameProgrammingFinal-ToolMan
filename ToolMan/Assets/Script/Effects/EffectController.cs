using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] private Transform baseTrans; // Transform of Player or Enemy, etc.
    public List<Effect> effectList = new List<Effect>();

    private void Awake()
    {
        // Set baseTrans of effects
        foreach (Effect e in effectList)
        {
            e.setBaseTransform(baseTrans);
        }
    }
}

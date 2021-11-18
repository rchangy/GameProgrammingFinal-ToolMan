using UnityEngine;
using System.Collections.Generic;

public class DamageCalculator : ScriptableObject
{
    [SerializeField]
    private List<TypeEffectiveness> typeEffects;

    private Dictionary<string, IReadOnlyDictionary<string, float>> typeEffectivenesses;

    private void Awake()
    {
        foreach(TypeEffectiveness e in typeEffects)
        {
            typeEffectivenesses.Add(e.GetDamageType(), e.GetEffectiveness());
        }
    }

    public float CalculateDmg(float baseDmg, CharacterStats damager, CharacterStats target)
    {
        var damagerTypes = damager.GetCurrentTypes();
        var targetTypes = target.GetCurrentTypes();
        float totalMultiplier = 1f;
        foreach(string damagerType in damagerTypes)
        {
            if (typeEffectivenesses.ContainsKey(damagerType))
            {
                var resistence = typeEffectivenesses[damagerType];
                foreach(string targetType in targetTypes)
                {
                    if (resistence.ContainsKey(targetType))
                    {
                        totalMultiplier *= resistence[targetType];
                    }
                }
            }
        }
        return baseDmg * totalMultiplier;
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace ToolMan.Combat
{
    [CreateAssetMenu(menuName = "ToolMan/Stat/DamageCalculator")]
    public class DamageCalculator : ScriptableObject
    {
        [SerializeField]
        private List<TypeEffectiveness> typeEffects;

        private Dictionary<string, IReadOnlyDictionary<string, float>> typeEffectivenesses;

        private bool loaded = false;


        public float CalculateDmg(float baseDmg, IReadOnlyCollection<string> damagerTypes, IReadOnlyCollection<string> targetTypes)
        {
            if (!loaded)
            {
                Load();
            }
            if (typeEffectivenesses == null) return baseDmg;
            float totalMultiplier = 1f;
            foreach (string damagerType in damagerTypes)
            {
                if (typeEffectivenesses.ContainsKey(damagerType))
                {
                    var resistence = typeEffectivenesses[damagerType];
                    foreach (string targetType in targetTypes)
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

        public void checkLoad()
        {
            if (!loaded) Load();
        }

        private void Load()
        {
            if (typeEffects == null) return;
            typeEffectivenesses = new Dictionary<string, IReadOnlyDictionary<string, float>>();
            foreach (TypeEffectiveness e in typeEffects)
            {
                if (e.GetDamageType() != null && e.GetEffectiveness() != null)
                {
                    if (!typeEffectivenesses.ContainsKey(e.GetDamageType()))
                        typeEffectivenesses.Add(e.GetDamageType(), e.GetEffectiveness());
                }
            }
            loaded = true;
        }
    }
}
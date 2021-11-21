using System.Collections.Generic;
using UnityEngine;
using System;
namespace ToolMan.Combat
{
    [CreateAssetMenu(menuName = "ToolMan/Stat/TypeEffectiveness")]
    public class TypeEffectiveness : ScriptableObject
    {
        [Serializable]
        private class Effectiveness
        {
            [SerializeField]
            public string ResistenceType;
            [SerializeField]
            public float Multiplier;
        }
        [SerializeField]
        private string _damageType;
        [SerializeField]
        private List<Effectiveness> _effectivenesses;

        private Dictionary<string, float> _effects;


        private void Awake()
        {
            if (_effectivenesses == null) return;
            foreach (Effectiveness e in _effectivenesses)
            {
                _effects.Add(e.ResistenceType, e.Multiplier);
            }
        }
        public string GetDamageType()
        {
            return _damageType;
        }

        public IReadOnlyDictionary<string, float> GetEffectiveness()
        {
            return _effects;
        }
    }
}
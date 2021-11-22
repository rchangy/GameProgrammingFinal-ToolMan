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
            public string ResistenceType = "";
            [SerializeField]
            public float Multiplier = 0f;
        }
        [SerializeField]
        private string _damageType;
        [SerializeField]
        private List<Effectiveness> _effectivenesses;

        private Dictionary<string, float> _effects;


        private void Awake()
        {
            if (_effectivenesses == null) return;
            _effects = new Dictionary<string, float>();
            foreach (Effectiveness e in _effectivenesses)
            {
                if(e.ResistenceType.Length > 0 && !_effects.ContainsKey(e.ResistenceType))
                { 
                    _effects.Add(e.ResistenceType, e.Multiplier);
                }
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
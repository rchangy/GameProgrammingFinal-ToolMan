using System;
using UnityEngine;
using ToolMan.Combat.Skills;


namespace ToolMan.Combat
{
    [Serializable]
    public class CombatModel
    {
        [SerializeField]
        private DamageCalculator _damageCalculator;
        public DamageCalculator DmgCalculator
        {
            get => _damageCalculator;
        }
        [SerializeField]
        private ComboSkillSet _comboSkillSet;
        public ComboSkillSet ComboSkills
        {
            get => _comboSkillSet;
        }
        [SerializeField]
        private HitFeel _hitFeel;
        public HitFeel hitFeel
        {
            get => _hitFeel;
        }
    }
}

using System;
using UnityEngine;
using ToolMan.Combat.Skills;

namespace ToolMan.Combat
{
    [Serializable]
    public class CombatModel
    {
        [SerializeField]
        private static DamageCalculator _damageCalculator;
        public DamageCalculator DmgCalculator
        {
            get => damageCalculator;
        }
        [SerializeField]
        private ComboSkillSet _comboSkillSet;
        public ComboSkillSet ComboSkills
        {
            get => _comboSkillSet;
        }

    }
}

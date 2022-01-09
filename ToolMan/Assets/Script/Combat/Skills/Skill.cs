using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ToolMan.Util;
using System;

/*
 * defines an attack
*/
namespace ToolMan.Combat.Skills
{
    public abstract class Skill : ScriptableObject
    {
        public float attackDelay;
        public float RefractoryPeriod;
        public float Cd;
        public float Multiplier = 1;
        public float PowMuliplier = 1;

        public abstract IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable);

        // for some effects when hit
        // default is empty
        public virtual IEnumerator Hit(SkillCombat combat, CombatUnit target)
        {
            Debug.Log("test");
            yield break;
        }

        public string getName()
        {
            return name;
        }

        public virtual void ResetCombatAttribute(SkillCombat combat)
        {

        }

        public virtual void ResetObject()
        {

        }
    }
}
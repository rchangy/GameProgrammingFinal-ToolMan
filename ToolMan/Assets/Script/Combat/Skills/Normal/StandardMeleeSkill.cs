﻿using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardMeleeSkill")]
    public class StandardMeleeSkill : Skill
    {
        //public float attackRange;
        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
        {
            // Animation
            anim.SetTrigger("Attack");
            Debug.Log("Perform Attack");
            // delay
            yield return new WaitForSeconds(attackDelay);
            collisionEnable.Value = true;
            // rotate
            collisionEnable.Value = false;
        }

    }
}
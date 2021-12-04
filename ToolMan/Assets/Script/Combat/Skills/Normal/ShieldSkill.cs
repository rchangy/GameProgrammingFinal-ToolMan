﻿using UnityEngine;
using System.Collections;
using ToolMan.Combat.Stats.Buff;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/Shield")]
    public class ShieldSkill : PlayerSkill
    {
        public ScriptableBuff Buff;

        public override IEnumerator Attack(PlayerController player, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
        {
            _toolCombat.AddBuff(Buff);
            _manCombat.AddBuff(Buff);
            // Animation
            player.AnimationAttack();
            // delay
            yield return new WaitForSeconds(attackDelay);
        }

    }
}
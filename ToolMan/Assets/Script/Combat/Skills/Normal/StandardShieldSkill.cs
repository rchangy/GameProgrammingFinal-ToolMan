using UnityEngine;
using System.Collections;
using ToolMan.Combat.Stats.Buff;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardShieldSkill")]
    public class StandardShieldSkill : Skill
    {
        public float attackRange;
        public ScriptableBuff Buff;
        private PlayerCombat anotherPlayerCombat;

        public override IEnumerator Attack(PlayerController player, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
        {
            if (typeof(PlayerCombat).IsInstanceOfType(combat))
            {
                var playerCombat = (PlayerCombat)combat;
                anotherPlayerCombat = playerCombat.TeamMateCombat;
            }
            combat.AddBuff(Buff);
            if (anotherPlayerCombat != null) { anotherPlayerCombat.AddBuff(Buff); }
            // Animation
            player.AnimationAttack();
            // delay
            yield return new WaitForSeconds(attackDelay);
        }

    }
}
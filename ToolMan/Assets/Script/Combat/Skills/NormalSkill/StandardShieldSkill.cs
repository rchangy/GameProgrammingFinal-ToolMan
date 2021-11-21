using UnityEngine;
using System.Collections;
using ToolMan.Combat.Stats.Buff;
    
namespace ToolMan.Combat.Skills.NormalSkill
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardShieldSkill")]
    public class StandardShieldSkill : Skill
    {
        public float attackRange;
        public ScriptableBuff Buff;
        private PlayerCombat anotherPlayerCombat;

        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat)
        {
            if (typeof(PlayerCombat).IsInstanceOfType(combat))
            {
                var playerCombat = (PlayerCombat)combat;
                anotherPlayerCombat = playerCombat.getAnotherPlayerCombat();
            }
            combat.AddBuff(Buff);
            if (anotherPlayerCombat != null) { anotherPlayerCombat.AddBuff(Buff); }
            // Animation
            anim.SetTrigger("Attack");
            // delay
            yield return new WaitForSeconds(attackDelay);
        }

    }
}
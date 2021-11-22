using UnityEngine;
using System.Collections;
using System;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardProjectileSkill")]
    public class StandardProjectileSkill : Skill
    {
        public float force;
        private Rigidbody rb;
        private PlayerController manPlayer;

        private bool exploded;
        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat)
        {
            rb = combat.gameObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.Log("Can't find tool's rigid body for projectile skill " + name);
                yield return null;
            }
            if (typeof(PlayerCombat).IsInstanceOfType(combat))
            {
                PlayerCombat toolCombat = (PlayerCombat)combat;
                manPlayer = toolCombat.TeamMateCombat.ThisPlayerController;
            }
            
            anim.SetTrigger("Attack");
            manPlayer.grabPoint.Release();
            exploded = false;
            yield return new WaitForSeconds(attackDelay);
            var dir = manPlayer.gameObject.transform.forward;
            dir.y = 1;
            rb.AddForce(dir * force);
            while (!exploded)
            {
                if (Input.GetButtonDown("JumpOrAttack1"))
                {
                    Debug.Log("Explosion");
                    exploded = true;
                }
                yield return null;
            }
        }

    }
}
using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardProjectileSkill")]
    public class StandardProjectileSkill : Skill
    {
        [SerializeField]
        private float _force;
        private Rigidbody rb;
        private PlayerController manPlayer;

        [SerializeField]
        private float _explosionRange;
        [SerializeField]
        private float _atkMultiplier;

        public override IEnumerator Attack(PlayerController player, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
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

            player.AnimationAttack();
            yield return new WaitForSeconds(attackDelay);
            manPlayer.Release();
            var dir = manPlayer.gameObject.transform.forward;
            dir.y = 1;
            rb.AddForce(dir * _force);
            while (true)
            {
                if (Input.GetButtonDown("JumpOrAttack1"))
                {
                    Debug.Log("Explosion");
                    // Check collisions
                    Collider[] hitTargets = Physics.OverlapSphere(rb.gameObject.transform.position, _explosionRange, targetLayer);
                    foreach (Collider target in hitTargets)
                    {
                        CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                        if (targetCombat != null)
                        {
                            targetCombat.TakeDamage((combat.Atk * _atkMultiplier), combat);
                        }
                    }
                    break;
                }
                yield return null;
            }
        }

    }
}
using UnityEngine;
using System.Collections;
using ToolMan.Util;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkill/FlashBomb")]
    public class FlashBombSkill : PlayerSkill
    {
        [SerializeField]
        private float _force;
        private Rigidbody rb;

        [SerializeField]
        private float _explosionRange;
        [SerializeField]
        private float _atkMultiplier;

        public override IEnumerator Attack(PlayerController player, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
        {
            rb = _tool.GetComponent<Rigidbody>();

            player.AnimationAttack();
            yield return new WaitForSeconds(attackDelay);
            _manController.Release();
            var dir = _man.transform.forward;
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
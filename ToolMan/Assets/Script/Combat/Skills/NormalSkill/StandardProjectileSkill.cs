using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ToolMan.Combat.Skills.NormalSkill
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardProjectileSkill")]
    public class StandardProjectileSkill : Skill
    {
        public Rigidbody rb;
        public Transform initPos;
        public float force;
        public PlayerController player;

        private bool exploded;
        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat)
        {
            anim.SetTrigger("Attack");
            player.grabPoint.Release();
            exploded = false;
            yield return new WaitForSeconds(attackDelay);
            var dir = initPos.forward;
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
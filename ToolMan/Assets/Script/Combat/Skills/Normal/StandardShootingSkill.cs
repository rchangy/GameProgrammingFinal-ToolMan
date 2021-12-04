using UnityEngine;
using System.Collections;
using ToolMan.Util;
namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardShootingSkill")]
    public class StandardShootingSkill : Skill
    {
        public GameObject bulletPrefab;
        public float shootingForce;
        private Transform attackPoint;

        public override IEnumerator Attack(PlayerController player, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable)
        {
            player.AnimationAttack();

            // wait for delay time
            yield return new WaitForSeconds(attackDelay);
            Shoot(combat.Atk, combat);
        }
        public virtual void Shoot(float atk, CombatUnit combat)
        {
            Debug.Log("Shoot");
            var bulletObject = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
            var bullet = bulletObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Atk = atk;
                bullet.shooter = combat;
            }

            var bulletRb = bulletObject.GetComponent<Rigidbody>();
            if (bulletRb != null) bulletRb.AddForce(attackPoint.forward * shootingForce);
        }
    }
}
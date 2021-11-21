using UnityEngine;
using System.Collections;
namespace ToolMan.Combat.Skills.NormalSkill
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/StandardShootingSkill")]
    public class StandardShootingSkill : Skill
    {
        public GameObject bulletPrefab;
        public float shootingForce;

        public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat)
        {
            anim.SetTrigger("Attack");

            // wait for delay time
            yield return new WaitForSeconds(attackDelay);
            Shoot((int)combat.Atk, combat);
        }
        public virtual void Shoot(int atk, CombatUnit combat)
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
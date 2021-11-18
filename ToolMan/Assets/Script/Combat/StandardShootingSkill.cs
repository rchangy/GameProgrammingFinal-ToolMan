using UnityEngine;
using System.Collections;


public class StandardShootingSkill : Skill
{ 
    public GameObject bulletPrefab;
    public float shootingForce;

    public override IEnumerator Attack(Animator anim, LayerMask targetLayer, Stat atk, Stat attackSpeed, CharacterStats stats)
    {
        anim.SetTrigger("Attack");

        // wait for delay time
        yield return new WaitForSeconds(attackDelay);
        Shoot((int)atk.Value);
    }
    public virtual void Shoot(int atk)
    {
        Debug.Log("Shoot");
        var bulletObject = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
        var bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null) bullet.Atk = atk;
        var bulletRb = bulletObject.GetComponent<Rigidbody>();
        if (bulletRb != null) bulletRb.AddForce(attackPoint.forward * shootingForce);
    }
}

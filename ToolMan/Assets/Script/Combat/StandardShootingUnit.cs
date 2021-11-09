using UnityEngine;
using System.Collections;

public class ShootingUnit : CombatUnit
{
    public Transform ShootingPoint;
    public float AttackDelay;
    public GameObject BulletPrefab;
    public float ShootingForce;

    public override void Attack()
    {
        if (HasAttacked) return;

        // Animation

        // delay
        StartCoroutine(ExecuteAfterTime(AttackDelay));

        Shoot();

        HasAttacked = true;
    }

    private void Shoot()
    {
        var bulletObject = Instantiate(BulletPrefab, ShootingPoint.position, ShootingPoint.rotation);
        var bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null) bullet.Atk = Atk;
        var bulletRb = bulletObject.GetComponent<Rigidbody>();
        if(bulletRb != null) bulletRb.AddForce(ShootingPoint.forward * ShootingForce);

    }
}

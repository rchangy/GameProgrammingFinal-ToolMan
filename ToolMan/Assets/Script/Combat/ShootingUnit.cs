using UnityEngine;
using System.Collections;

public class ShootingUnit : CombatUnit
{
    public Transform ShootingPoint;
    public float AttackDelay;
    public Rigidbody BulletPrefab;
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
        var bullet = Instantiate(BulletPrefab, ShootingPoint.position, ShootingPoint.rotation);
        bullet.AddForce(ShootingPoint.forward * ShootingForce);
    }
}

using UnityEngine;


public abstract class ShootingSkill : Skill
{ 
    public Transform ShootingPoint;
    public float AttackDelay;
    public GameObject BulletPrefab;
    public float ShootingForce;

}

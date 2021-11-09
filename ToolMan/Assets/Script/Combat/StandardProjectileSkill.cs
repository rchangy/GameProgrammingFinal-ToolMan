using UnityEngine;
using System.Collections;

public class StandardProjectileSkill : Skill
{
    public Rigidbody rb;
    public Transform initPos;
    public float force;
    private bool exploded;
    public override IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat, int Atk)
    {
        anim.SetTrigger("Attack");
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
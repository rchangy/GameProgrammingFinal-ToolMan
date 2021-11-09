using UnityEngine;
using System.Collections;

/*
 * defines an attack
*/
public abstract class Skill : MonoBehaviour
{
    public string SkillName;
    public float attackInterval;
    public float attackDelay;
    // shooting point or attack point
    public Transform attackPoint;


    public abstract IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat , int Atk);


    
}

using UnityEngine;
using System.Collections;

/*
 * defines an attack
*/
public abstract class Skill : MonoBehaviour
{
    public float attackInterval;
    public float attackDelay;
    // shooting point or attack point
    protected Transform attackPoint
    {
        get => gameObject.transform;
    }

    public string getName()
    {
        return gameObject.name;
    }

    public abstract IEnumerator Attack(Animator anim, LayerMask targetLayer, Stat atk, Stat attackSpeed, CharacterStats stats);


    
}

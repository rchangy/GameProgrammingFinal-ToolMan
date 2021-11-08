using UnityEngine;
using System.Collections;

/*
 * defines an attack
*/
[System.Serializable]
public abstract class Skill
{
    public int a;
    public abstract void Attack(int Atk);
    protected IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "ToolMan/Disability")]
public class ScriptableDisability : ScriptableBuff
{
    public override void AddBuff(CharacterStats target)
    {
        if (target.HasBuff(this)) target.AddBuff(this, null);
        else target.AddBuff(this, new TimedDisability(this, target));
    }

    

}
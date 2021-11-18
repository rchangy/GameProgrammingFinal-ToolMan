using UnityEngine;

[CreateAssetMenu(menuName = "ToolMan/StatBuff")]
public class ScriptableStatBuff : ScriptableBuff
{
    public StatModType ModType;
    public float Value;
    /**
     * Effect value is increased each time the buff is applied.
     */
    public bool IsEffectStacked;
    public int MaxEffectStack;

    public override void AddBuff(CharacterStats target)
    {
        if (target.HasBuff(this)) target.AddBuff(this, null);
        else target.AddBuff(this, new TimedStatBuff(this, target));
    }

}
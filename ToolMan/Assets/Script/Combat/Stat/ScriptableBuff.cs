using UnityEngine;

public abstract class ScriptableBuff : ScriptableObject
{

    public string Target;

    /**
     * Time duration of the buff in seconds.
     */
    public float Duration;

    /**
     * Duration is increased each time the buff is applied.
     */
    public bool IsDurationStacked;
    public int MaxDuration;

    public abstract void AddBuff(CharacterStats target);
}
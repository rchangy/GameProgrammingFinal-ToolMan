using UnityEngine;
namespace ToolMan.Combat.Stats.Buff
{
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

        public ScriptableBuff(string target, float duration, bool isDurationStacked, int maxDuration)
        {
            Target = target;
            Duration = duration;
            IsDurationStacked = isDurationStacked;
            MaxDuration = maxDuration;
        }
        public abstract void AddBuff(CharacterStats target);

        public abstract void RemoveBuff(CharacterStats stats);

    }
}
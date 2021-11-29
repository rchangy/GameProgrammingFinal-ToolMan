using UnityEngine;
namespace ToolMan.Combat.Stats.Buff
{
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
        public ScriptableStatBuff(string target, float duration, bool isDurationStacked, int maxDuration, StatModType modType, float value, bool isEffectStacked, int maxEffectStack)
            : base(target, duration, isDurationStacked, maxDuration)
        {
            ModType = modType;
            Value = value;
            IsEffectStacked = isEffectStacked;
            MaxEffectStack = maxEffectStack;
        }

        public override void AddBuff(CharacterStats target)
        {
            if (target.HasBuff(this)) target.AddBuff(this, null);
            else target.AddBuff(this, new TimedStatBuff(this, target));
        }

    }
}
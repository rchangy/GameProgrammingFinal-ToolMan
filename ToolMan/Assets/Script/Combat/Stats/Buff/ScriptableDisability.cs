using UnityEngine;
namespace ToolMan.Combat.Stats.Buff
{
    [CreateAssetMenu(menuName = "ToolMan/Disability")]
    public class ScriptableDisability : ScriptableBuff
    {
        public ScriptableDisability(string target, float duration, bool isDurationStacked, int maxDuration)
            :base(target, duration, isDurationStacked, maxDuration)
        {

        }
        public override void AddBuff(CharacterStats target)
        {
            if (target.HasBuff(this)) target.AddBuff(this, null);
            else target.AddBuff(this, new TimedDisability(this, target));
        }
    }
}
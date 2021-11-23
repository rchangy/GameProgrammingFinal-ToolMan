using UnityEngine;
using System.Collections.Generic;

namespace ToolMan.Combat.Stats.Buff
{
    public class TimedStatBuff : TimedBuff
    {
        //protected float Duration;
        private ScriptableStatBuff _statBuff { get; }
        protected int EffectStacks;
        private Dictionary<StatModifier, float> activeMods = new Dictionary<StatModifier, float>();
        private List<StatModifier> _toBeRemoved = new List<StatModifier>();
        private List<StatModifier> _toBeActivated = new List<StatModifier>();
        private int _activationStack = 0;
        public TimedStatBuff(ScriptableStatBuff buff, CharacterStats target) : base(buff, target)
        {
            _statBuff = buff;
        }

        public override void Tick(float delta)
        {
            List<StatModifier> keys = new List<StatModifier>(activeMods.Keys);
            foreach (StatModifier mod in keys)
            {
                activeMods[mod] += _activationStack * _statBuff.Duration;
                activeMods[mod] = Mathf.Min(activeMods[mod], Buff.MaxDuration);
                activeMods[mod] -= delta;
                if (activeMods[mod] <= 0)
                {
                    RemoveEffect(mod);
                    _toBeRemoved.Add(mod);
                }
            }
            _activationStack = 0;

            foreach (StatModifier mod in _toBeActivated)
            {
                if (ApplyEffect(mod))
                {
                    activeMods.Add(mod, _statBuff.Duration);
                }

            }
            _toBeActivated.Clear();

            foreach (StatModifier mod in _toBeRemoved)
            {
                activeMods.Remove(mod);
            }
            _toBeRemoved.Clear();

            if (activeMods.Count == 0)
            {
                //End();
                IsFinished = true;
            }
        }

        /**
         * Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
         */
        public override void Activate()
        {
            StatModifier newMod = null;
            if ((_statBuff.IsEffectStacked && EffectStacks < _statBuff.MaxEffectStack) || newBuff)
            {
                if (Target.HasStat(_statBuff.Target))
                {
                    newMod = new StatModifier(_statBuff.Value, _statBuff.ModType, this);
                    _toBeActivated.Add(newMod);
                    EffectStacks++;
                    newBuff = false;
                }
            }

            if (Buff.IsDurationStacked)
            {
                _activationStack++;
            }
        }


        protected bool ApplyEffect(StatModifier mod)
        {
            if (Target.HasStat(_statBuff.Target))
            {
                var stat = Target.GetStatByName(_statBuff.Target);
                stat.AddModifier(mod);
                return true;
            }
            return false;
        }

        protected void RemoveEffect(StatModifier mod)
        {
            if (Target.HasStat(_statBuff.Target))
            {
                var stat = Target.GetStatByName(_statBuff.Target);
                stat.RemoveModifier(mod);
                EffectStacks--;
            }
        }

    }
}
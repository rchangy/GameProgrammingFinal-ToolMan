using UnityEngine;
using System.Collections.Generic;

namespace ToolMan.Combat.Stats.Buff
{
    public abstract class TimedBuff
    {
        //protected float Duration;
        protected bool newBuff = true;
        public ScriptableBuff Buff { get; }
        protected readonly CharacterStats Target;
        public bool IsFinished = false;

        public TimedBuff(ScriptableBuff buff, CharacterStats target)
        {
            Buff = buff;
            Target = target;
        }

        public abstract void Tick(float delta);

        /**
         * Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
         */
        public abstract void Activate();

    }
}
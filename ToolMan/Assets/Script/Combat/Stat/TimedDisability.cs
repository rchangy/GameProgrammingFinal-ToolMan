using UnityEngine;
using System.Collections.Generic;


public class TimedDisability : TimedBuff
{
    private float _duration;

    public TimedDisability(ScriptableBuff buff, CharacterStats target) : base(buff, target)
    {

    }

    public override void Tick(float delta)
    {
        _duration -= delta;
        if(_duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    /**
     * Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
     */
    public override void Activate()
    {
        if (newBuff)
        {
            if (Target.HasAbility(Buff.Target))
            {
                _duration = Buff.Duration;
                var ability = Target.GetAbilityByName(Buff.Target);
                if (ability != null)
                {
                    ability.Disable();
                }
            }
        }
        if (Buff.IsDurationStacked || !newBuff)
        {
            _duration += Buff.Duration;
            _duration = Mathf.Min(_duration, Buff.MaxDuration);
        }
    }


    public void End()
    {
        if (Target.HasAbility(Buff.Target))
        {
            _duration = Buff.Duration;
            var ability = Target.GetAbilityByName(Buff.Target);
            if (ability != null)
            {
                ability.Disable();
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;


public class TimedStatBuff : TimedBuff
{
    //protected float Duration;
    private ScriptableStatBuff _statBuff { get; }
    protected int EffectStacks;
    private Dictionary<StatModifier, float> activeMods = new Dictionary<StatModifier, float>();
    private List<StatModifier> toBeRemoved = new List<StatModifier>();

    public TimedStatBuff(ScriptableStatBuff buff, CharacterStats target) : base(buff, target)
    {

    }

    public override void Tick(float delta)
    {
        foreach (StatModifier mod in activeMods.Keys)
        {
            activeMods[mod] -= delta;
            if(activeMods[mod] <= 0)
            {
                RemoveEffect(mod);
                toBeRemoved.Add(mod);
            }
        }

        foreach(StatModifier mod in toBeRemoved)
        {
            activeMods.Remove(mod);
        }
        toBeRemoved.Clear();

        if(activeMods.Count == 0)
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
            newMod = ApplyEffect();
            EffectStacks++;
        }

        if (Buff.IsDurationStacked || newBuff)
        {
            foreach (StatModifier mod in activeMods.Keys)
            {
                if (mod == newMod) continue;    // new modifier don't need to refresh
                activeMods[mod] += Buff.Duration;
                activeMods[mod] = Mathf.Min(activeMods[mod], Buff.MaxDuration);
            }
        }
    }


    protected StatModifier ApplyEffect(){
        if (Target.HasStat(_statBuff.Target))
        {
            var mod = new StatModifier(_statBuff.Value, _statBuff.ModType, this);
            var stat = Target.GetStatByName(_statBuff.Target);
            if(stat != null)
            {
                stat.AddModifier(mod);
                activeMods.Add(mod, Buff.Duration);
            }
            return mod;
        }
        return null;
    }

    protected void RemoveEffect(StatModifier mod)
    {
        if (Target.HasStat(_statBuff.Target))
        {
            var stat = Target.GetStatByName(_statBuff.Target);
            stat.RemoveModifier(mod);
        }
    }

}
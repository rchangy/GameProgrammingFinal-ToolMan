using UnityEngine;

public class TurtleBuffEffect : Effect
{
    ParticleSystem[] _partList;

    public override void PlayEffect()
    {
        if (_partList == null || _partList.Length == 0)
        {
            _partList = transform.GetComponentsInChildren<ParticleSystem>();
        }
        if (_partList == null) return;
        foreach (ParticleSystem child in _partList)
        {
            child.Play();
        }
    }

    public override void StopEffect()
    {
        if (_partList == null || _partList.Length == 0)
        {
            _partList = transform.GetComponentsInChildren<ParticleSystem>();
        }
        if (_partList == null) return;
        foreach (ParticleSystem child in _partList)
        {
            child.Stop();
        }
    }
}

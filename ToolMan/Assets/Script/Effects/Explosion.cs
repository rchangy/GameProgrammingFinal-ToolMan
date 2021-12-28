using UnityEngine;

public class Explosion : Effect
{
    private bool _hasPlayed = false;
    public bool DestroyAfterPlaying;
    ParticleSystem[] _partList;

    public override void PlayEffect()
    {
        if(_partList == null || _partList.Length == 0)
        {
            _partList = transform.GetComponentsInChildren<ParticleSystem>();
        }
        if (_partList == null) return;
        foreach (ParticleSystem child in _partList) {
            child.Play();
        }
        _hasPlayed = true;
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

    private void Update()
    {
        if (!DestroyAfterPlaying) return;
        if (_hasPlayed)
        {
            foreach (ParticleSystem child in _partList)
            {
                if (child.isPlaying) return;
            }
            Destroy(gameObject);
        }
    }
}

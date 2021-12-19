using UnityEngine;

public class Explosion : Effect
{
    private bool _hasPlayed = false;
    public bool DestroyAfterPlaying;
    ParticleSystem[] _partList;

    public override void PlayEffect()
    {
        _partList = transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem child in _partList) {
            child.Play();
        }
        _hasPlayed = true;
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

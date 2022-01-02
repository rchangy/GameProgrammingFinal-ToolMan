using UnityEngine;
using System.Collections;

public class Explosion : Effect
{
    private bool _isPlaying;
    public bool isPlaying
    {
        get => _isPlaying;
    }
    private bool _hasPlayed = false;
    public bool DestroyAfterPlaying;
    ParticleSystem[] _partList;
    public bool _hasLight = false;
    Light[] _lightList;

    public IEnumerator PlayForSeconds(float duration)
    {
        PlayEffect();
        yield return new WaitForSeconds(duration);
        StopEffect();
    }

    public override void PlayEffect()
    {
        if(_partList == null || _partList.Length == 0)
        {
            _partList = transform.GetComponentsInChildren<ParticleSystem>();
        }
        if(_hasLight && (_lightList == null || _lightList.Length == 0))
        {
            _lightList = transform.GetComponentsInChildren<Light>();
        }
        if (_partList == null) return;
        foreach (ParticleSystem child in _partList) {
            child.Play();
        }
        if(_lightList != null)
        {
            foreach (Light child in _lightList)
            {
                child.enabled = true;
            }
        }
        _hasPlayed = true;
        _isPlaying = true;
    }

    public override void StopEffect()
    {
        if (_partList == null || _partList.Length == 0)
        {
            _partList = transform.GetComponentsInChildren<ParticleSystem>();
        }
        if (_hasLight && (_lightList == null || _lightList.Length == 0))
        {
            _lightList = transform.GetComponentsInChildren<Light>();
        }
        if (_partList == null) return;
        foreach (ParticleSystem child in _partList)
        {
            child.Stop();
        }
        if (_lightList != null)
        {
            foreach (Light child in _lightList)
            {
                child.enabled = false;
            }
        }
        _isPlaying = false;
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

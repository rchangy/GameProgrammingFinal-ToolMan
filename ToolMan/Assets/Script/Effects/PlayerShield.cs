using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private Material _mat;
    [SerializeField] private Material _matBig;
    [SerializeField] private RectTransform _big1;
    [SerializeField] private RectTransform _big2;

    public bool Big;

    private Vector3 _startScale = new Vector3(0.8f, 0.8f, 0.8f);
    private Vector3 _endScale = new Vector3(1.1f, 1.1f, 1.1f);

    private Color _startEffectColor = new Color(1, 0.4f, 0.47f, 0.6f);
    private Color _startEffectColorBig = new Color(1, 0.4f, 0.47f, 0.2f);
    private Color _endEffectColor = new Color(1, 0.4f, 0.47f, 0f);

    [SerializeField] private float _fadeInTime;
    [SerializeField] private float _fadeOutTime;

    private bool _fading;

    void Start()
    {
        _mat.SetColor("_Color", _endEffectColor);
    }

    public void Fire(float duration)
    {
        if (_fading) return;
        if(Big)
            StartCoroutine(FireEffectBig());
        else
            StartCoroutine(FireEffect(duration));
    }

    private IEnumerator FireEffect(float duration)
    {
        _fading = true;
        float timePassed = 0f;
        while (timePassed < _fadeInTime)
        {
            _mat.SetColor("_Color", Color.Lerp(_endEffectColor, _startEffectColor, timePassed / _fadeInTime));
            timePassed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        timePassed = 0f;
        while (timePassed < _fadeOutTime)
        {
            _mat.SetColor("_Color", Color.Lerp(_startEffectColor, _endEffectColor, timePassed / _fadeOutTime));
            timePassed += Time.deltaTime;
            yield return null;
        }
        _mat.SetColor("_Color", _endEffectColor);
        _fading = false;
    }

    private IEnumerator FireEffectBig()
    {
        float timePassed = 0f;
        while (timePassed < _fadeInTime)
        {
            _big1.localScale = Vector3.Lerp(_startScale, _endScale, timePassed / _fadeInTime);
            _big2.localScale = Vector3.Lerp(_startScale, _endScale, timePassed / _fadeInTime);
            _matBig.SetColor("_Color", Color.Lerp(_endEffectColor, _startEffectColorBig, timePassed / _fadeInTime));
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;
        while (timePassed < _fadeOutTime)
        {
            _matBig.SetColor("_Color", Color.Lerp(_startEffectColorBig, _endEffectColor, timePassed / _fadeOutTime));
            timePassed += Time.deltaTime;
            yield return null;
        }
        _matBig.SetColor("_Color", _endEffectColor);
    }
}

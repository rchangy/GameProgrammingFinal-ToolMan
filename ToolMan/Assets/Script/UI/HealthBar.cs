using UnityEngine;
using UnityEngine.UI;
using ToolMan.Combat.Stats;


public class HealthBar : MonoBehaviour
{
    private Image _barSprite;

    private Resource _hp;
    private float _lastHealth;
    private bool isSet = false;

    private void Start()
    {
        _barSprite = gameObject.GetComponent<Image>();
        _barSprite.enabled = false;
    }

    private void Update()
    {
        if (isSet)
        {
            if(_hp.Value != _lastHealth)
            {
                _barSprite.fillAmount = _hp.Value / _hp.MaxValue;
                _lastHealth = _hp.Value;
            }
        }
    }

    public void Setup(Resource hp)
    {
        _barSprite.enabled = true;
        _hp = hp;
        _lastHealth = _hp.Value;
        isSet = true;
    }
}

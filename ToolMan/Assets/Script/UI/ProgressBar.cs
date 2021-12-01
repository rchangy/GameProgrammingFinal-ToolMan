using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ToolMan.Util;

[RequireComponent(typeof(Image))]
public class ProgressBar : MonoBehaviour
{
    private Image _barSprite;
    private bool isSet = false;

    private BoolWrapper _rendererEnable;
    private FloatWrapper _maxValue;
    private FloatWrapper _value;


    void Start()
    {
        _barSprite = gameObject.GetComponent<Image>();
        _barSprite.fillAmount = 0;
        _barSprite.enabled = false;
    }

    private void Update()
    {
        if (isSet)
        {
            _barSprite.enabled = _rendererEnable.Value;
            _barSprite.fillAmount = Mathf.Min(_value.Value / _maxValue.Value, 1);
        }
    }

    public void Setup(BoolWrapper rendererEnable, FloatWrapper maxValue, FloatWrapper value)
    {
        _rendererEnable = rendererEnable;
        _maxValue = maxValue;
        _value = value;
        isSet = true;
    }
}

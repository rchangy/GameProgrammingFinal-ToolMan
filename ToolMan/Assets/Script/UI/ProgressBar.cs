using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ToolMan.Util;

[RequireComponent(typeof(Image))]
public class ProgressBar : MonoBehaviour
{
    private Image _barSprite;
    //private Renderer _renderer;
    private bool isSet = false;


    private BoolWrapper _rendererEnable;
    private FloatWrapper _maxValue;
    private FloatWrapper _value;


    void Start()
    {
        //_renderer = gameObject.GetComponent<Renderer>();
        //_renderer.enabled = false;
        gameObject.SetActive(false);
        _barSprite = gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (isSet)
        {
            gameObject.SetActive(_rendererEnable.Value);
            _barSprite.fillAmount = Mathf.Max(_value.Value / _maxValue.Value, 1);
        }
    }

    public void Setup(BoolWrapper rendererEnable, FloatWrapper maxValue, FloatWrapper value)
    {
        _rendererEnable = rendererEnable;
        _maxValue = maxValue;
        _value = value;
    }
}

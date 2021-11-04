using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _barSprite;
    private int _maxHealth;

    public void SetMaxHealth(int health)
    {
        _maxHealth = health;
        _barSprite.fillAmount = 1f;
    }

    public void SetHealth(int health)
    {
        _barSprite.fillAmount = health / _maxHealth;
    }
}

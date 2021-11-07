using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class CombatUnit : MonoBehaviour
{
    public int MaxHealth;
    public HealthBar healthBar;
    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Max(0, value);
            _health = Mathf.Min(MaxHealth, _health);
            // change health bar
            if(healthBar != null) healthBar.SetHealth(_health);
            if (_health <= 0) Die();
        }

    }
    
    public LayerMask TargetLayers;

    public int Atk;
    
    public float AttackInterval;
    protected float _timeToNextAttack;
    protected bool _hasAttacked;
    protected bool HasAttacked
    {
        get => _hasAttacked;
        set
        {
            _hasAttacked = value;
            if (_hasAttacked) _timeToNextAttack = AttackInterval;
        }
    }

    public Animator Anim;


    protected virtual void Start()
    {
        if(healthBar != null) healthBar.SetMaxHealth(MaxHealth);
        Health = MaxHealth;
        _hasAttacked = false;
    }

    private void FixedUpdate()
    {
        if (_hasAttacked)
        {
            _timeToNextAttack -= Time.deltaTime;
            if(_timeToNextAttack <= 0)
            {
                _hasAttacked = false;
            }
        }
    }

    public virtual void Attack()
    {
        if (_hasAttacked) return;

        // do attack

        _hasAttacked = true;
    }

    public virtual void TakeDamage(int dmg)
    {
        Debug.LogFormat("[{0}] Took {1} Damage", name, dmg);
        Health -= dmg;
    }

    public virtual void Healed(int heal)
    {
        Debug.LogFormat("[{0}] Healed {1}", name, heal);
        Health += heal;
    }

    public virtual void Die()
    {
        Debug.Log(name + " dies");
        Destroy(gameObject);
    }

    // for attack delay
    protected IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}

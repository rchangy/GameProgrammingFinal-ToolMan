using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;


[Serializable]
public class Ability
{
    [SerializeField]
    private string _statName;
    public bool BaseValue = true;

    private int _disability;

    private bool isDirty = true;

    [SerializeField]
    private bool _value;
    [SerializeField]
    public bool Value
    {
        get
        {
            if (isDirty)
            {
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }


    public Ability()
    {
        _disability = 0;
    }


    public string getName()
    {
        return _statName;
    }


    public void Disable()
    {
        _disability += 1;
    }

    public void RemoveDisability()
    {
        _disability -= 1;
    }

    private bool CalculateFinalValue()
    {
        if (_disability > 0) return false;
        else return true;
    }

}


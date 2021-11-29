using UnityEngine;
using System.Collections;

using ToolMan.Combat.Equip;

public class ShieldEnemy : MonoBehaviour
{
    public float triggerTime;
    public GameObject ShieldPrefab;
    public Material mat;

    private bool shieldAdded = false;

    private void Update()
    {
        triggerTime -= Time.deltaTime;
        if(triggerTime <= 0 && !shieldAdded)
        {
            AddShield();
            shieldAdded = true;
        }
    }

    private void AddShield()
    {
        var s = Instantiate(ShieldPrefab, transform);
        ForceShield shield = s.GetComponent<ForceShield>();
    }
}

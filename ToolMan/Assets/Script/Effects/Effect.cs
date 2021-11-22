using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    [SerializeField] protected GameObject effect;

    Transform baseTransform;
    [SerializeField] Vector3 offset = Vector3.zero; // The effect will be played at (base transform + offset)

    [SerializeField] bool triggered = false;
    
    public void setBaseTransform(Transform trans) { baseTransform = trans; }

    protected void Update()
    {
        CheckIfTriggerd(); // The effect check by itself
        // Or, other class can call SetTriggered()

        if(triggered)
            PlayEffect();
    }

    private void LateUpdate()
    {
        triggered = false;
    }

    public abstract void PlayEffect();

    protected void CheckIfTriggerd() { }

    public void SetTriggered(bool val) { triggered = val; }
}

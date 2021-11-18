using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    [SerializeField] protected GameObject effect;

    Transform baseTransform;
    [SerializeField] Vector3 offset = Vector3.zero; // The effect will be played at (base transform + offset)

    public void setBaseTransform(Transform trans) { baseTransform = trans; }

    public abstract void PlayEffect();
}

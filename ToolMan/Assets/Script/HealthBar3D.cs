using UnityEngine;

public class HealthBar3D : MonoBehaviour
{
    [SerializeField] private Transform _targetObject;
    [SerializeField] private Transform _healthBar;
    [SerializeField] private Camera _cam;
    [SerializeField] private Vector3 _posOffset;
    private void LateUpdate()
    {
        _healthBar.position = _cam.WorldToScreenPoint(_targetObject.position + _posOffset);
        Debug.Log(_targetObject.position);
    }
}

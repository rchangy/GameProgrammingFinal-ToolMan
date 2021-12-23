using UnityEngine;

public class EnemyHPCanvas : MonoBehaviour
{
    [SerializeField] private Transform cam;
    private void LateUpdate()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.forward);
    }

    public void setCamera(Transform camera)
    {
        this.cam = camera;
    }
}

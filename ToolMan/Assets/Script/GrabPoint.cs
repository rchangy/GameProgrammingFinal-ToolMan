using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    public float grabRange = 1.5f;
    public LayerMask toolLayer;

    GameObject targetTool;
    public bool grabbing = false;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
   }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (grabbing)
                Release();
            else
                Grab();
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange, toolLayer);
        foreach (Collider collider in colliders) { targetTool = collider.gameObject; }
        if (colliders.Length == 0)
            targetTool = null;
        //else
        //    Debug.Log("sth to grab:))");
    }

    private void Grab() {
        if (targetTool != null) {
            FixedJoint fj = targetTool.AddComponent<FixedJoint>();
            fj.connectedBody = rb;
            fj.breakForce = 2147483847;

            grabbing = true;
            Debug.Log("grab");
        }
    }

    private void Release() {
        if (targetTool != null) {
            Destroy(targetTool.GetComponent<FixedJoint>());
            
            grabbing = false;
            Debug.Log("release");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}

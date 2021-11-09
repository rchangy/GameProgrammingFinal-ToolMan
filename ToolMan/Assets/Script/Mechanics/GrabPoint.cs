using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    public float grabRange = 1.5f;
    public LayerMask grabbedPointLayer;
    GameObject targetTool;
    public bool grabbing = false;

    Rigidbody rb;
    PlayerController player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void setPlayer(PlayerController p) { player = p;  }

    // Update is called once per frame
    void Update()
    {
        if (!player.isTool)
        {
            if ( (Input.GetButtonDown("Grab1")&&(player.playerNum == 1) ) || (Input.GetButtonDown("Grab2")&&(player.playerNum == 2)) )
            {
                if (grabbing)
                    Release();
                else
                    Grab();
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange, grabbedPointLayer);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                    targetTool = collider.gameObject;
            }
            if (colliders.Length == 0)
                targetTool = null;
        }
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

            // Reset grabbed player rigidbody
            targetTool.GetComponent<GrabbedPoint>().resetRigidBody();

            // Reset grabbing player rigidbody
            player.transform.Find("GrabbedPoint").GetComponent<GrabbedPoint>().resetRigidBody();

            grabbing = false;
            Debug.Log("release");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}

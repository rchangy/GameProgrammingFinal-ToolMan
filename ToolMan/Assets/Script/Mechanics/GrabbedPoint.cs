using UnityEngine;

public class GrabbedPoint : MonoBehaviour
{
    public GameObject player;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        FixedJoint fj = player.AddComponent<FixedJoint>();
        fj.connectedBody = rb;
        fj.breakForce = 2147483647;
    }

    public void resetRigidBody()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        Debug.Log("ccc");
    }
}

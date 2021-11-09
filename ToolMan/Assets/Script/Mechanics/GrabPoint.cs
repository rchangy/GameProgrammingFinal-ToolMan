using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    public float grabRange = 1.5f;
    public LayerMask grabbedPointLayer;
    GameObject targetTool;
    public bool grabbing = false;
    PlayerController anotherPlayer;

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
                Debug.Log("Press Grab1");
                if (grabbing)
                    Release();
                else
                    Grab();
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange, grabbedPointLayer);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    anotherPlayer = collider.transform.parent.gameObject.GetComponent<PlayerController>();
                    if(anotherPlayer != null && anotherPlayer.isTool) targetTool = collider.gameObject;
                }
                    
            }
            if (colliders.Length == 0 && !grabbing)
            {
                targetTool = null;
                //Debug.Log("set TargetTool to Null");
            }
        }
    }

    private void Grab() {
        Debug.Log("In Grab");
        if (targetTool != null) {
            FixedJoint fj = targetTool.AddComponent<FixedJoint>();
            fj.connectedBody = rb;
            fj.breakForce = 2147483847;

            // set Tool in the status of being grabbed
            anotherPlayer.beGrabbed();
            anotherPlayer.grabbedPointClass.setAnotherPlayer(player);
            //GameObject hand = player.RightHand;
            //anotherPlayer.grabbedPoint.transform.position = hand.transform.position;
            //anotherPlayer.transform.forward = player.transform.forward;

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

            // set Tool in the status of being released
            anotherPlayer.beReleased();
            anotherPlayer.grabbedPointClass.setAnotherPlayer(null);

            grabbing = false;
            Debug.Log("release");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}

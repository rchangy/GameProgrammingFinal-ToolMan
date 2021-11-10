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
                {
                    Debug.Log("Press Release");
                    Release();
                }
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
            // set FixedJoint
            FixedJoint fj = gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = anotherPlayer.getRigidbody();
            fj.breakForce = 2147483847;
            fj.autoConfigureConnectedAnchor = false;
            //fj.connectedAnchor = anotherPlayer.grabbedPoint.transform.localPosition;
            fj.connectedAnchor = anotherPlayer.getTool().getPoint();
            //Debug.Log("getPoint() = " + anotherPlayer.getTool().getPoint() + ", localPosition = " + anotherPlayer.grabbedPoint.transform.localPosition);
            fj.enableCollision = false;

            // set Tool in the status of being grabbed
            anotherPlayer.grabbedPointClass.setAnotherPlayer(player);
            anotherPlayer.beGrabbed();

            grabbing = true;
            Debug.Log("grab");
        }
    }

    private void Release() {
        if (targetTool != null) {
            //Destroy(targetTool.GetComponent<FixedJoint>());
            Destroy(gameObject.GetComponent<FixedJoint>());
            // Reset grabbed player rigidbody
            targetTool.GetComponent<GrabbedPoint>().resetRigidBody();

            // Reset grabbing player rigidbody
            player.grabbedPointClass.resetRigidBody();

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

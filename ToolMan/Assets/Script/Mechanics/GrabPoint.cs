using UnityEngine;

namespace ToolMan.Mechanics
{
    public class GrabPoint : MonoBehaviour
    {
        public float grabRange = 1.5f;
        public LayerMask grabbedPointLayer;
        GameObject targetTool;
        public bool grabbing = false;
        private PlayerController anotherPlayer;

        Rigidbody rb;
        PlayerController player;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void setPlayer(PlayerController p) { player = p; }

        // Update is called once per frame
        void Update()
        {
            if (!player.inToolState())
            {
                if ((Input.GetButtonDown("Grab1") && (player.playerNum == 1)) || (Input.GetButtonDown("Grab2") && (player.playerNum == 2)))
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
                        if (anotherPlayer != null && anotherPlayer.inToolState()) targetTool = collider.gameObject;
                    }

                }
                if (colliders.Length == 0 && !grabbing)
                {
                    targetTool = null;
                }
            }
        }

        public void Grab()
        {
            if (targetTool != null)
            {
                // set FixedJoint
                FixedJoint fj = gameObject.AddComponent<FixedJoint>();
                fj.connectedBody = anotherPlayer.getRigidbody();
                fj.breakForce = 2147483847;
                fj.autoConfigureConnectedAnchor = false;
                fj.connectedAnchor = anotherPlayer.getTool().getPoint();
                fj.enableCollision = false;

                // set Tool in the status of being grabbed
                anotherPlayer.beGrabbed(player);

                grabbing = true;
                Debug.Log("grab");
            }
        }

        public void Release()
        {
            if (targetTool != null)
            {
                //Destroy(targetTool.GetComponent<FixedJoint>());
                Destroy(gameObject.GetComponent<FixedJoint>());
                // Reset grabbed player rigidbody
                targetTool.GetComponent<GrabbedPoint>().resetRigidBody();

                // Reset grabbing player rigidbody? not sure if need this
                player.Release();

                // set Tool in the status of being released
                anotherPlayer.beReleased();

                grabbing = false;
                Debug.Log("release");
            }
        }

        public void setAnotherPlayerAndTarget(PlayerController anotherPlayer)
        {
            this.anotherPlayer = anotherPlayer;
            this.targetTool = anotherPlayer.getGrabbedPoint();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, grabRange);
        }
    }
}
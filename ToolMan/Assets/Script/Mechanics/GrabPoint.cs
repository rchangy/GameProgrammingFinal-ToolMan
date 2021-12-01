using UnityEngine;

namespace ToolMan.Mechanics
{
    public class GrabPoint : MonoBehaviour
    {
        [SerializeField] private float grabRange = 1.5f;
        [SerializeField] private LayerMask grabbedPointLayer;
        
        private bool grabbing = false;
        private GameObject targetPoint; // Tool's grabbed point
        private PlayerController player;
        private PlayerController anotherPlayer;

        // unused?
        private Rigidbody rb;
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void setPlayer(PlayerController p)
        {
            player = p;
        }

        // Update is called once per frame
        void Update()
        {
            if (!player.inToolState())
            {
                DetectCollisionWithGrabbedPoint();
            }
        }

        public void GrabOrRelease()
        {
            if (grabbing)
            {
                Debug.Log("Press Release");
                Release();
            }
            else
            {
                Debug.Log("Press Grab");
                Grab();
            }
        }

        private void DetectCollisionWithGrabbedPoint()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange, grabbedPointLayer);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    anotherPlayer = collider.gameObject.GetComponent<GrabbedPoint>().GetPlayerController();
                    if (anotherPlayer != null && anotherPlayer.inToolState())
                    {
                        Debug.Log("can grab grabbed : )");
                        targetPoint = collider.gameObject;
                    }
                }

            }
            if (colliders.Length == 0 && !grabbing)
            {
                targetPoint = null;
            }
        }

        public void Grab()
        {
            if (targetPoint != null)
            {
                // set FixedJoint
                FixedJoint fj = gameObject.AddComponent<FixedJoint>();
                //FixedJoint fj = player.gameObject.AddComponent<FixedJoint>();
                fj.connectedBody = anotherPlayer.getRigidbody();
                fj.breakForce = 2147483847;
                fj.autoConfigureConnectedAnchor = false;
                //fj.anchor = player.GetRightHand().transform.localPosition;
                fj.connectedAnchor = anotherPlayer.getTool().getPoint();
                fj.enableCollision = false;

                // set Tool in the status of being grabbed
                anotherPlayer.BeGrabbed(player);

                grabbing = true;
                Debug.Log("grab");
            }
        }

        public void Release()
        {
            if (targetPoint != null)
            {
                //Destroy(targetTool.GetComponent<FixedJoint>());
                Destroy(gameObject.GetComponent<FixedJoint>());
                // Reset grabbed player rigidbody
                targetPoint.GetComponent<GrabbedPoint>().resetRigidBody();

                // Reset grabbing player rigidbody? not sure if need this
                player.Release();

                // set Tool in the status of being released
                anotherPlayer.BeReleased();

                grabbing = false;
                Debug.Log("release");
            }
        }

        public void setAnotherPlayerAndTarget(PlayerController anotherPlayer)
        {
            this.anotherPlayer = anotherPlayer;
            this.targetPoint = anotherPlayer.GetGrabbedPoint().gameObject;
        }

        public bool IsGrabbing()
        {
            return grabbing;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, grabRange);
        }
    }
}
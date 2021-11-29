using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public Vector3 manPosOffset;
    public Vector3 toolPosOffset;
    [SerializeField] private Quaternion prevRotation;
    [SerializeField] private Quaternion originalRotation;
    [SerializeField] private bool playerIsTool = false;

    public void setIsTool(bool val) { playerIsTool = val; }

    private void Awake()
    {
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (playerIsTool)
        {
            transform.localPosition = toolPosOffset;
            transform.rotation = prevRotation;
            // Look at
            //Debug.Log("prev = " + prevPosition_y);
        }
        else
        {
            transform.localPosition = manPosOffset;
            prevRotation = transform.rotation;
        }
    }

    //private void LateUpdate()
    //{
    //    if (playerIsTool)
    //    {
    //        transform.position = new Vector3(transform.position.x, prevPosition_y, transform.position.z);
    //        Debug.Log("prev, trans = " + transform.position);
    //    }
    //}

}

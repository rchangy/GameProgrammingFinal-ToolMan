using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPoint : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tool"))
            targetTool = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tool"))
            targetTool = null;
    }
}

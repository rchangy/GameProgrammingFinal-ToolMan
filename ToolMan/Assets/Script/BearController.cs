using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BearController : MonoBehaviour
{

    private Animator animator;

    public Transform mainCameraTrans;

    [SerializeField] private float speed = 5;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.sqrMagnitude > 0.01f)
        {
            // Facing angle (smoothed)
            float movementAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCameraTrans.eulerAngles.y;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, movementAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);

            // Move
            Vector3 adjustedMovement = Quaternion.Euler(0, movementAngle, 0) * Vector3.forward; // Relative to mainCameraTrans
            adjustedMovement *= speed * Time.deltaTime;
            transform.position += adjustedMovement;
        }

    }
}

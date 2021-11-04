using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class BearController : MonoBehaviour
{

    [SerializeField] private CharacterController characterController;
    private Animator animator;
    public string hi = "hi";

    /*[SerializeField] private float movementSpeed, rotationSpeed, gravity;

    private Vector3 movementDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Debug.Log("start: " + transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        Vector3 inputMovement = transform.forward * movementSpeed * Input.GetAxisRaw("Vertical");
        characterController.Move(inputMovement * Time.deltaTime);

        transform.Rotate(Vector3.up * Input.GetAxisRaw("Horizontal") * rotationSpeed);

        // gravity (position.y will change according to the height of ground)
        movementDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movementDirection * Time.deltaTime);

        animator.SetBool("isWalking", Input.GetAxisRaw("Vertical") != 0);
    }*/

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
            Debug.Log(adjustedMovement.magnitude);
            characterController.Move(adjustedMovement * speed * Time.deltaTime);

        }

    }
}

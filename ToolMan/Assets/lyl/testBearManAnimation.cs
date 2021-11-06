using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBearManAnimation : ToolableMan
{
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        toMan();
        animator.SetFloat("velocity_x", 0.0f);
        animator.SetBool("inFlight", false);
        animator.SetBool("isGrounded", true);
        animator.SetBool("isSpinning", false);
        animator.SetBool("Attacking", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            toMan();
        }
        else if (Input.GetKey(KeyCode.P))
        {
            toPickaxe();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            toShield();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("Attacking", true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("Attacking", false);
        }
    }
}

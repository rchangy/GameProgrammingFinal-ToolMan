using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class testCorgiAnimation : MonoBehaviour
{
    protected Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetTrigger("attack");
        }
        else if (Input.GetKey(KeyCode.H))
        {
            animator.SetTrigger("hurt");
        }
    }
}

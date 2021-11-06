using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ToolableMan : MonoBehaviour
{
    protected Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void toPickaxe()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isPickaxe", true);
    }

    protected void toBoomerang()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isBoomerang", true);
    }

    protected void toSword()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isSword", true);
    }

    protected void toFlashBomb()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isFlashBomb", true);
    }

    protected void toShield()
    {
        animator.SetBool("isTool", true);
        animator.SetBool("isShield", true);
    }

    protected void toMan()
    {
        animator.SetBool("isTool", false);
        animator.SetBool("isShield", false);
        animator.SetBool("isFlashBomb", false);
        animator.SetBool("isSword", false);
        animator.SetBool("isBoomerang", false);
        animator.SetBool("isPickaxe", false);
    }
}

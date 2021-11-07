using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBearManAnimation : MonoBehaviour
{
    protected Animator animator;
    protected GameObject grabbedPoint;
    /* Pickaxe, Shield, */
    List<Tool> tools = new List<Tool>();
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        grabbedPoint = gameObject.transform.Find("GrabbedPoint").gameObject;
        toMan();
        animator.SetFloat("velocity_x", 0.0f);
        animator.SetBool("inFlight", false);
        animator.SetBool("isGrounded", true);
        animator.SetBool("isSpinning", false);
        animator.SetBool("Attacking", false);
        tools.Add(new Pickaxe(animator, grabbedPoint));
        tools.Add(new Shield(animator, grabbedPoint));
        tools.Add(new FlashBomb(animator, grabbedPoint));
        tools.Add(new Boomerang(animator, grabbedPoint));
        tools.Add(new LightSaber(animator, grabbedPoint));
        Debug.Log(tools[0].getGrabbedPoint().transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(tools[0].getGrabbedPoint().transform.localPosition);
        if (Input.GetKey(KeyCode.M))
        {
            toMan();
        }
        else if (Input.GetKey(KeyCode.P))
        {
            tools[0].toTool();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            tools[1].toTool();
        }
        else if (Input.GetKey(KeyCode.F))
        {
            tools[2].toTool();
        }
        else if (Input.GetKey(KeyCode.B))
        {
            tools[3].toTool();
        }
        else if (Input.GetKey(KeyCode.W))
        {
            tools[4].toTool();
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
    protected void toMan()
    {
        animator.SetBool("isTool", false);
        animator.SetBool("isShield", false);
        animator.SetBool("isFlashBomb", false);
        animator.SetBool("isSword", false);
        animator.SetBool("isBoomerang", false);
        animator.SetBool("isPickaxe", false);
        grabbedPoint.transform.localPosition = new Vector3(0.0f, -1.2f, 0.0f);
    }
}

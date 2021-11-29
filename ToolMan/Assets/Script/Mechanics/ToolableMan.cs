using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolMan.Mechanics;

[RequireComponent(typeof(Animator))]
public abstract class ToolableMan : MonoBehaviour
{
    public bool isTool = false;
    protected List<Tool> tools = new List<Tool>();
    protected int toolIdx;

    [SerializeField] protected GameObject grabbedPoint;
    [SerializeField] protected GrabbedPoint grabbedPointController;

    virtual protected void Awake() {}

    virtual protected void Start() {}

    virtual protected void Update() {}

    abstract public void ToolableManTransform();

    // ==== grab/grabbed ====

    public void Release()
    {
        grabbedPointController.resetRigidBody();
    }

    public void beGrabbed(PlayerController anotherPlayer)
    {
        tools[toolIdx].beGrabbed();
        grabbedPoint.GetComponent<Collider>().isTrigger = true;
        //gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        gameObject.GetComponent<Collider>().isTrigger = true;
        grabbedPointController.setAnotherPlayer(anotherPlayer);
    }

    public void beReleased()
    {
        tools[toolIdx].beReleased();
        grabbedPoint.GetComponent<Collider>().isTrigger = false;
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.GetComponent<Collider>().isTrigger = false;
        grabbedPointController.setAnotherPlayer(null);
    }

    // ==== getters ====
    public GameObject getGrabbedPoint()
    {
        return grabbedPoint;
    }

    public Tool getTool()
    {
        return tools[toolIdx];
    }

    public bool inToolState()
    {
        return isTool;
    }
}

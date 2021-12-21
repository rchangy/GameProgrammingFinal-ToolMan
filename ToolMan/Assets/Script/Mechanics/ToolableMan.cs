using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolMan.Mechanics;

[RequireComponent(typeof(Animator))]
public abstract class ToolableMan : MonoBehaviour
{
    protected bool isTool = false;
    protected List<Tool> tools = new List<Tool>();
    protected int toolIdx;
    protected bool beGrabbed = false;

    [SerializeField] protected GameObject grabbedPoint;

    virtual protected void Awake() {}

    virtual protected void Start() {}

    virtual protected void Update() {}

    abstract public void ToolableManTransform();

    abstract public void BeGrabbed(PlayerController anotherPlayer);

    abstract public void BeReleased();

    // ==== getters ====

    public Tool getTool()
    {
        return tools[toolIdx];
    }

    public bool inToolState()
    {
        return isTool;
    }
    public GameObject GetGrabbedPoint()
    {
        return grabbedPoint;
    }
    public bool IsGrabbed()
    {
        return beGrabbed;
    }
    // ==== getters ====
}

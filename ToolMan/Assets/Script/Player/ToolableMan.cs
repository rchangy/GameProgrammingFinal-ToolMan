using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolMan.Player;
using ToolMan.Util;

[RequireComponent(typeof(Animator))]
public abstract class ToolableMan : MonoBehaviour
{
    protected BoolWrapper isTool = new BoolWrapper();
    protected List<Tool> tools = new List<Tool>();
    protected IntegerWrapper toolIdx = new IntegerWrapper();
    protected bool beGrabbed = false;

    [SerializeField] protected ObjectListUI toolListUI;

    [SerializeField] protected GameObject grabbedPoint;

    virtual protected void Awake() {}

    virtual protected void Start() { }
    virtual protected void Update() {}

    abstract public void ToolableManTransform();

    abstract public void BeGrabbed(PlayerController anotherPlayer);

    abstract public void BeReleased();

    public void SetUpToolList()
    {
        toolListUI.SetUp(toolIdx, isTool);
    }

    // ==== getters ====

    public Tool getTool()
    {
        return tools[toolIdx.Value];
    }

    public bool inToolState()
    {
        return isTool.Value;
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

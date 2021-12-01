using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputController
{
    public bool JumpOrAttack(int playerNum);
    public float MoveHorizontal(int playerNum);
    public float MoveVertical(int playerNum);
    public bool Choose(int playerNum);
    public bool NextTool(int playerNum);
    public bool PrevTool(int playerNum);
    public bool GrabOrRelease(int playerNum);
}

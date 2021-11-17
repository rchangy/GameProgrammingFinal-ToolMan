using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputController : InputController
{
    public bool JumpOrAttack(int playerNum)
    {
        if (playerNum == 1)
        {
            if (Input.GetButtonDown("JumpOrAttack1"))
                return true;
        }
        else if (playerNum == 2)
        {
            if (Input.GetButtonDown("JumpOrAttack2"))
                return true;
        }
        return false;
    }

    public float MoveHorizontal(int playerNum)
    {
        if (playerNum == 1)
        {
            float h = Input.GetAxisRaw("Horizontal1");
            Debug.Log("playerNum = " + playerNum + " h = " + h);
            return h;
        }
        else
            return Input.GetAxisRaw("Horizontal2");
    }

    public float MoveVertical(int playerNum)
    {
        if (playerNum == 1)
            return Input.GetAxisRaw("Vertical1");
        else
            return Input.GetAxisRaw("Vertical2");
    }

    public bool Choose(int playerNum)
    {
        if ((Input.GetButtonDown("Choose1") && playerNum == 1) || (Input.GetButtonDown("Choose2") && playerNum == 2))
            return true;
        return false;
    }
}

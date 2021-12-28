using UnityEngine;
using System.Collections;
using ToolMan.Mechanics;

public class TutorialMoveAndTrans : TutorialController
{
    [SerializeField] private PlayerController _player1;
    [SerializeField] private PlayerController _player2;

    private bool _isDetectingPlayerMove;
    private bool _player1MoveComplete;
    private bool _player2MoveComplete;

    private bool _isDetectingPlayerJump;
    private bool _player1JumpComplete;
    private bool _player2JumpComplete;

    private bool _isDetectingTrans1;
    private bool _player1ToToolComplete;
    private bool _player1ToManComplete;
    private bool _isDetectingTrans2;
    private bool _player2ToToolComplete;
    private bool _player2ToManComplete;


    private void Update()
    {
        if (_isDetectingPlayerMove)
        {
            if(!_player1MoveComplete && (Input.GetButtonDown("Horizontal1") || Input.GetButtonDown("Vertical1")))
            {
                _player1MoveComplete = true;
            }
            if(!_player2MoveComplete && (Input.GetButtonDown("Horizontal2") || Input.GetButtonDown("Vertical2")))
            {
                _player2MoveComplete = true;
            }

            if(_player1MoveComplete && _player2MoveComplete)
            {
                // complete move task
                _isDetectingPlayerMove = false;
            }
        }
        if (_isDetectingPlayerJump)
        {
            if (!_player1JumpComplete && Input.GetButtonDown("Jump1"))
            {
                _player1JumpComplete = true;
            }
            if (!_player2JumpComplete && Input.GetButtonDown("Jump2"))
            {
                _player2JumpComplete = true;
            }
            if(_player1JumpComplete && _player2JumpComplete)
            {
                // complete jump task
                _isDetectingPlayerJump = false;
            }
        }
        if (_isDetectingTrans1)
        {
            if(!_player1ToToolComplete && _player1.inToolState())
            {
                _player1ToToolComplete = true;
            }
            if(_player1ToToolComplete && !_player1ToManComplete && !_player1.inToolState())
            {
                _player1ToManComplete = true;
            }
            if(_player1ToToolComplete && _player1ToManComplete)
            {
                //
                _isDetectingTrans1 = false;
            }
        }
        if (_isDetectingTrans2)
        {

        }
    }
    
    protected override IEnumerator TutorialProcess()
    {
        _isTalkEnd = false;
        rpgTalk.NewTalk("32", "34");
        while(!_isTalkEnd)
            yield return null;
        _isDetectingPlayerMove = true;
        _isDetectingPlayerJump = true;
        while(_isDetectingPlayerJump || _isDetectingPlayerMove)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        _isTalkEnd = false;
        rpgTalk.NewTalk("37", "39");
        while (!_isTalkEnd)
            yield return null;

    }
}

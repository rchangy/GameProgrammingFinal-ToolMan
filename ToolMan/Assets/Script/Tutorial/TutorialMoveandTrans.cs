using UnityEngine;
using System.Collections;
using ToolMan.Player;

public class TutorialMoveAndTrans : TutorialController
{
    [SerializeField] private PlayerController _player1;
    [SerializeField] private PlayerController _player2;

    public bool taskMove;
    public bool taskJump;
    public bool taskTransform;
    public bool taskToolTurnView;

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

    private bool _isDetectingToolTurnView;
    private bool _player1ToolTurnViewComplete;
    private bool _player2ToolTurnViewComplete;


    private void Update()
    {
        if (_isDetectingPlayerMove)
        {
            DetectPlayerMove();
        }
        if (_isDetectingPlayerJump)
        {
            DetectPlayerJump();
        }
        if (_isDetectingTrans1 || _isDetectingTrans2)
        {
            DetectTrans();
        }
        else if (_isDetectingToolTurnView)
        {
            DetectToolTurnView();
        }
        
    }

    private void DetectPlayerMove()
    {
        if (!_player1MoveComplete && (Input.GetButtonDown("Horizontal1") || Input.GetButtonDown("Vertical1")))
        {
            _player1MoveComplete = true;
        }
        if (!_player2MoveComplete && (Input.GetButtonDown("Horizontal2") || Input.GetButtonDown("Vertical2")))
        {
            _player2MoveComplete = true;
        }

        if (_player1MoveComplete && _player2MoveComplete)
        {
            // complete move task
            _isDetectingPlayerMove = false;
        }
    }

    private void DetectPlayerJump()
    {
        if (!_player1JumpComplete && Input.GetButtonDown("JumpOrAttack1"))
        {
            _player1JumpComplete = true;
        }
        if (!_player2JumpComplete && Input.GetButtonDown("JumpOrAttack1"))
        {
            _player2JumpComplete = true;
        }
        if (_player1JumpComplete && _player2JumpComplete)
        {
            // complete jump task
            _isDetectingPlayerJump = false;
        }
    }

    private void DetectTrans()
    {
        if (_isDetectingTrans1)
        {
            if (!_player1ToToolComplete && _player1.inToolState())
            {
                _player1ToToolComplete = true;
            }
            if (_player1ToToolComplete && !_player1ToManComplete && !_player1.inToolState())
            {
                _player1ToManComplete = true;
            }
            if (_player1ToToolComplete && _player1ToManComplete)
            {
                // complete transform task
                _isDetectingTrans1 = false;
            }
        }
        if (_isDetectingTrans2)
        {
            if (!_player2ToToolComplete && _player2.inToolState())
            {
                _player2ToToolComplete = true;
            }
            if (_player2ToToolComplete && !_player2ToManComplete && !_player2.inToolState())
            {
                _player2ToManComplete = true;
            }
            if (_player2ToToolComplete && _player2ToManComplete)
            {
                // complete transform task
                _isDetectingTrans2 = false;
            }
        }
    }

    private void DetectToolTurnView()
    {
        if (!_player1ToolTurnViewComplete && _player1.inToolState() && (Input.GetButtonDown("Horizontal1") || Input.GetButtonDown("Vertical1")))
        {
            _player1ToolTurnViewComplete = true;
        }
        if (!_player2ToolTurnViewComplete && _player2.inToolState() && (Input.GetButtonDown("Horizontal2") || Input.GetButtonDown("Vertical2")))
        {
            _player2ToolTurnViewComplete = true;
        }
        if (_player1ToolTurnViewComplete && _player2ToolTurnViewComplete)
        {
            // complete turn view task
            _isDetectingToolTurnView = false;
        }
    }
    
    protected override IEnumerator TutorialProcess()
    {
        //_isTalkEnd = false;
        //rpgTalk.NewTalk("32", "34");
        //while(!_isTalkEnd)
        //    yield return null;
        uIController.SetBattleUI(true);
        _player1.controlEnable = true;
        _player2.controlEnable = true;
        _isDetectingPlayerMove = taskMove;
        _isDetectingPlayerJump = taskJump;
        _isDetectingTrans1 = taskTransform;
        _isDetectingTrans2 = taskTransform;
        _isDetectingToolTurnView = taskToolTurnView;
        while (_isDetectingPlayerJump || _isDetectingPlayerMove || _isDetectingTrans1 || _isDetectingTrans2 || _isDetectingToolTurnView)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        _player1.controlEnable = false;
        _player2.controlEnable = false;
        Debug.Log("control enable -> false");
        uIController.SetBattleUI(false);
        //battleCanvas.gameObject.SetActive(false);
        //_isTalkEnd = false;
        //rpgTalk.NewTalk("37", "39");
        //while (!_isTalkEnd)
        //    yield return null;

    }
}

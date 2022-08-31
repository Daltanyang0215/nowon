using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLadder : StateMachineBase
{
    private LadderDetector _ladderDetector;
    private GroundDetector _groundDetector;
    private CapsuleCollider2D _col;
    private Rigidbody2D _rb;
    public StateMachineLadder(StateMachineManager.State machineState, StateMachineManager manager, AnimationManager animationManager) : base(machineState, manager, animationManager)
    {
        _ladderDetector = manager.GetComponent<LadderDetector>();
        _groundDetector = manager.GetComponent<GroundDetector>();
        _col = manager.GetComponent<CapsuleCollider2D>();
        _rb = manager.GetComponent<Rigidbody2D>();
    }

    public override void Execute()
    {
        manager.isMovable = false;
        manager.isDirectionChangable = false;
        animationManager.speed = 0.0f;
        manager.ResetVelocity();
        state = State.Prepare;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void FixedUpdateState()
    {
    }

    public override void ForceStop()
    {
        animationManager.speed = 1f;
        state = State.Idle;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public override bool IsExecuteOk()
    {
        bool isOK = false;
        if ((_ladderDetector.isGoUpPassible || _ladderDetector.isGoDownPassible) &&
            (manager.state == StateMachineManager.State.Idle ||
            manager.state == StateMachineManager.State.Move ||
            manager.state == StateMachineManager.State.Jump ||
            manager.state == StateMachineManager.State.Fall ||
            manager.state == StateMachineManager.State.Dash))
            isOK = true;
        return isOK;
    }

    public override StateMachineManager.State UpdateState()
    {
        StateMachineManager.State nextState = managerState;
        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                animationManager.Play("Ladder");
                if (_ladderDetector.isGoUpPassible)
                {
                    _rb.position = new Vector2(_ladderDetector.ladderBottomPoint.x , _rb.position.y + _col.size.y * 0.1f);

                }else if (_ladderDetector.isGoDownPassible)
                {
                    _rb.position = new Vector2(_ladderDetector.ladderBottomPoint.x , _rb.position.y - _col.size.y * 0.5f);

                }
                state = State.onAction;
                break;
            case State.Casting:
                break;
            case State.onAction:
                float v = Input.GetAxisRaw("Vertical");
                animationManager.speed =Mathf.Abs( v);
                _rb.MovePosition(_rb.position + Vector2.up * v * Time.deltaTime*5.0f);

                if(_rb.position.y+_col.size.y*0.5f < _ladderDetector.ladderBottomPoint.y
                    ||( _rb.position.y < _ladderDetector.ladderBottomPoint.y && _groundDetector.isDetected))
                {
                    state++;
                }else if(_rb.position.y > _ladderDetector.ladderTopPoint.y-0.1f)
                {
                    _rb.position = _ladderDetector.ladderTopPoint;
                    state++;
                }

                if (Input.GetKeyDown(KeyCode.X) && manager.h != 0)
                {
                    if(manager.h < 0)
                    {
                        manager.direction = -1;
                    }
                    else if(manager.h >0) {
                        manager.direction = 1;
                    }

                        nextState = StateMachineManager.State.Jump;
                        manager.ForceChangeState(StateMachineManager.State.Jump);
                        manager._move.x = manager.h;
                }

                break;
            case State.Finish:
                nextState = StateMachineManager.State.Idle;
                break;
            case State.Error:
                break;
            case State.WaitForErrorClear:
                break;
            default:
                break;
        }
        return nextState;
    }
}

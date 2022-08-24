using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineDash : StateMachineBase
{
    private Rigidbody2D _rb;
    private float _animationTime;
    private float _animationTimer;
    private float _dashSpeed = 2.0f;
    public StateMachineDash(StateMachineManager.State machineState, StateMachineManager manager, AnimationManager animationManager) : base(machineState, manager, animationManager)
    {
        _rb = manager.GetComponent<Rigidbody2D>();
        _animationTime = animationManager.GetAnimationTime("Dash");
        shortKey = KeyCode.V;
    }

    public override void Execute()
    {
        manager.isMovable = false;
        manager.isDirectionChangable = false;
        state = State.Prepare;
    }

    public override void FixedUpdateState()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                break;
            case State.Casting:
                _rb.velocity = Vector2.right * manager.direction * _dashSpeed * 0.5f;
                break;
            case State.onAction:
                _rb.velocity = Vector2.right * manager.direction * _dashSpeed;
                break;
            case State.Finish:
                _rb.velocity = Vector2.right * manager.direction * _dashSpeed * 0.5f;
                break;
            case State.Error:
                break;
            case State.WaitForErrorClear:
                break;
            default:
                break;
        }
    }

    public override void ForceStop()
    {
        state = State.Idle;
    }

    public override bool IsExecuteOk()
    {
        bool isOK = false;
        if (manager.state == StateMachineManager.State.Idle || manager.state == StateMachineManager.State.Move || manager.state == StateMachineManager.State.Jump || manager.state == StateMachineManager.State.Fall)
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
                animationManager.Play("Dash");
                _animationTimer = _animationTime;
                state++;
                break;
            case State.Casting:
                if (_animationTimer < _animationTime * 3.5f / 4f)
                {
                    state++;
                }
                else
                {
                    _animationTimer -= Time.deltaTime;
                }
                break;
            case State.onAction:
                if (_animationTimer < _animationTime * 1.5f / 4f)
                {
                    state++;
                }
                else
                {
                    _animationTimer -= Time.deltaTime;
                }
                break;
            case State.Finish:
                if (_animationTimer < 0)
                {
                    nextState = StateMachineManager.State.Idle;
                }
                else
                {
                    _animationTimer -= Time.deltaTime;
                }
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

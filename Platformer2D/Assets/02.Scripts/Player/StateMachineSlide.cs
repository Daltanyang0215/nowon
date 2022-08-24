using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSlide : StateMachineBase
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private float _animationTime;
    private float _animationTimer;
    private float _slideSpeed=2.0f;
    private Vector2 _colOffsetOrigin;
    private Vector2 _colSizeOrigin;
     private Vector2 _colOffsetCrouch = new Vector2(0, 0.055f);
    private Vector2 _colSizeCrouch = new Vector2(0.11f, 0.11f);
    public StateMachineSlide(StateMachineManager.State machineState, StateMachineManager manager, AnimationManager animationManager) : base(machineState, manager, animationManager)
    {
        _rb = manager.GetComponent<Rigidbody2D>();
        _col = manager.GetComponent<CapsuleCollider2D>();
        _animationTime = animationManager.GetAnimationTime("Slide");
        _colOffsetOrigin = _col.offset;
        _colSizeOrigin = _col.size;
        shortKey = KeyCode.C;
    }

    public override void Execute()
    {
        manager.isMovable = false;
        manager.isDirectionChangable = false;
        _col.size = _colSizeCrouch;
        _col.offset = _colOffsetCrouch;
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
                _rb.velocity = Vector2.right * manager.direction * _slideSpeed * 0.5f;
                break;
            case State.onAction:
                _rb.velocity = Vector2.right * manager.direction * _slideSpeed;
                break;
            case State.Finish:
                _rb.velocity = Vector2.right * manager.direction * _slideSpeed* 0.5f;
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
        _col.size = _colSizeOrigin;
        _col.offset = _colOffsetOrigin;

        state = State.Idle;
    }

    public override bool IsExecuteOk()
    {
        bool isOK = false;
        if (manager.state == StateMachineManager.State.Idle || manager.state == StateMachineManager.State.Move)
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
                animationManager.Play("Slide");
                _animationTimer = _animationTime;
                state++;
                break;
            case State.Casting:
                if (_animationTimer < _animationTime * 3f / 4f)
                {
                    state++;
                }
                else
                {
                    _animationTimer -= Time.deltaTime;
                }
                break;
            case State.onAction:
                if (_animationTimer < _animationTime * 1f / 4f)
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

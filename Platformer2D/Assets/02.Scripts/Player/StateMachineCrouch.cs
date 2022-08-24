using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineCrouch : StateMachineBase
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private Vector2 _colOffsetOrigin;
    private Vector2 _colSizeOrigin;
    private Vector2 _colOffsetCrouch = new Vector2(0, 0.055f);
    private Vector2 _colSizeCrouch = new Vector2(0.11f, 0.11f);
    public StateMachineCrouch(StateMachineManager.State machineState, StateMachineManager manager, AnimationManager animationManager) : base(machineState, manager, animationManager)
    {
        _rb = manager.GetComponent<Rigidbody2D>();
        _col = manager.GetComponent<CapsuleCollider2D>();
        _colOffsetOrigin = _col.offset;
        _colSizeOrigin = _col.size;
        shortKey = KeyCode.DownArrow;
    }

    public override void Execute()
    {
        manager.isMovable = false;
        manager.isDirectionChangable = true;
        _col.size = _colSizeCrouch;
        _col.offset = _colOffsetCrouch;
        state = State.Prepare;
    }

    public override void FixedUpdateState()
    {
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
                animationManager.Play("Crouch");
                manager.ResetVelocity();
                state=State.onAction;
                break;
            case State.Casting:
                break;
            case State.onAction:
                if (Input.GetKeyUp(shortKey))
                {
                    nextState = StateMachineManager.State.Idle;
                }
                break;
            case State.Finish:
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

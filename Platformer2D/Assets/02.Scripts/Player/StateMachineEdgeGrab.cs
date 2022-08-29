using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineEdgeGrab : StateMachineBase
{
    private enum EdgeType
    {
        EdgeGrab,
        EdgeIdle,
        EdgeClimb,
        EdgeSlide
    }
    private EdgeType _edgeType;

    private EdgeDetector _edgeDetector;
    private GroundDetector _groundDetector;
    private Rigidbody2D _rb;
    private float _edgeGrabAnimationTime;
    private float _edgeClimbAnimationTime;
    private float _animationTimer;

    public StateMachineEdgeGrab(StateMachineManager.State machineState, StateMachineManager manager, AnimationManager animationManager) : base(machineState, manager, animationManager)
    {
        _edgeDetector = manager.GetComponent<EdgeDetector>();
        _rb = manager.GetComponent<Rigidbody2D>();
        _groundDetector = manager.GetComponent<GroundDetector>();
        _edgeGrabAnimationTime = animationManager.GetAnimationTime("EdgeGrab");
        _edgeClimbAnimationTime = animationManager.GetAnimationTime("EdgeClimb");
    }

    public override void Execute()
    {
        manager.isMovable = false;
        manager.isDirectionChangable = false;
        manager.ResetVelocity();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _edgeType = EdgeType.EdgeGrab;
        state = State.Prepare;
    }

    public override void FixedUpdateState()
    {
    }

    public override void ForceStop()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        state = State.Idle;
    }

    public override bool IsExecuteOk()
    {
        bool isOK = false;
        if (_edgeDetector.isDetected)
            isOK = true;
        return isOK;
    }

    public override StateMachineManager.State UpdateState()
    {
        switch (_edgeType)
        {
            case EdgeType.EdgeGrab:
                return EdgeGrabWorkflow();
            case EdgeType.EdgeIdle:
                return EdgeIdleWorkflow();
            case EdgeType.EdgeClimb:
                return EdgeClimbWorkflow();
            case EdgeType.EdgeSlide:
                return EdgeSlideWorkflow();
            default:
                throw new System.Exception("¿§Áö±×·¦ »óÅÂ ºÒ·®");
        }

    }

    private StateMachineManager.State EdgeGrabWorkflow()
    {
        StateMachineManager.State nextState = managerState;
        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                animationManager.Play("EdgeGrab");
                _animationTimer = _edgeGrabAnimationTime;
                state = State.onAction;
                break;
            case State.Casting:
                break;
            case State.onAction:
                if (_animationTimer < 0)
                {
                    state++;
                }
                else
                {
                    _animationTimer -= Time.deltaTime;
                }
                break;
            case State.Finish:
                _edgeType = EdgeType.EdgeIdle;
                state = State.Prepare;
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
    private StateMachineManager.State EdgeIdleWorkflow()
    {
        StateMachineManager.State nextState = managerState;
        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                animationManager.Play("EdgeIdle");
                state = State.onAction;
                break;
            case State.Casting:
                break;
            case State.onAction:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {

                    _edgeType = EdgeType.EdgeClimb;
                    state = State.Prepare;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    _edgeType = EdgeType.EdgeSlide;
                    state = State.Prepare;
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
    private StateMachineManager.State EdgeClimbWorkflow()
    {
        StateMachineManager.State nextState = managerState;
        Vector2 slerpCenter=Vector2.zero;
        Vector2 startpos=Vector2.zero;
        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                animationManager.Play("EdgeClimb");
                _animationTimer = _edgeClimbAnimationTime;
                slerpCenter = new Vector2(_edgeDetector.climbPos.x * 2 - _rb.position.x, _rb.position.y * 2f - _edgeDetector.climbPos.y);
                startpos = _rb.position;
                state = State.onAction;
                break;
            case State.Casting:
                break;
            case State.onAction:
                if (_animationTimer < 0)
                {
                    state++;
                }
                else
                {
                    //_rb.MovePosition((Vector2)Vector3.Slerp(_rb.position - slerpCenter, _edgeDetector.climbPos - slerpCenter, (_edgeClimbAnimationTime - _animationTimer / _edgeClimbAnimationTime))+ slerpCenter);
                    _rb.MovePosition(Vector3.Slerp(startpos, _edgeDetector.climbPos , (_edgeClimbAnimationTime - _animationTimer / _edgeClimbAnimationTime)));
                    _animationTimer -= Time.deltaTime;
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
    private StateMachineManager.State EdgeSlideWorkflow()
    {
        StateMachineManager.State nextState = managerState;
        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                animationManager.Play("EdgeSlide");
                _rb.bodyType = RigidbodyType2D.Dynamic;
                state = State.onAction;
                break;
            case State.Casting:
                break;
            case State.onAction:
                if (_groundDetector.isDetected)
                {
                    state++;
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

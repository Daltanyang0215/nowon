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
    float _slideSpeed= -1.5f;

    Vector2 _climbpos = Vector2.zero;

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

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    _edgeType = EdgeType.EdgeSlide;
                    state = State.Prepare;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {

                    _edgeType = EdgeType.EdgeClimb;
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

        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                animationManager.Play("EdgeClimb");
                _animationTimer = _edgeClimbAnimationTime;
                _climbpos = _edgeDetector.climbPos;
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
                    if(_rb.position.y < _climbpos.y)
                    {
                        _rb.position += Vector2.up * Time.deltaTime / _edgeClimbAnimationTime;
                    }else //if(Mathf.Abs(_rb.position.x - _climbpos.x) > 0.01f)
                    {
                        _rb.position += Vector2.right * manager.direction * Time.deltaTime / (_edgeClimbAnimationTime*4);
                    }

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
                //_rb.bodyType = RigidbodyType2D.Dynamic;
                state = State.onAction;
                break;
            case State.Casting:
                break;
            case State.onAction:
                if (_groundDetector.isDetected)
                {
                    state++;
                }
                else
                {
                    _rb.MovePosition(_rb.position - (Vector2.down * Time.deltaTime * _slideSpeed)) ;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Move,
        Jump,
        Attack,
        Dash,
        Slide
    }
    private enum IdleState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }
    private enum MoveState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }

    public State state;
    [SerializeField]private IdleState _idleState;
    [SerializeField]private MoveState _moveState;

    private Vector2 _move;
    [SerializeField] private float _moveSpeed =1;

    // -1 : left ,+1 : right
    private int _direction;
    public int direction
    {
        get { return _direction; }
        set { 
            if (value < 0)
            {
                _direction = -1;
                transform.eulerAngles = new Vector3(0f,180f,0f);
            }
            else
            {
                _direction = 1;
                transform.eulerAngles = Vector3.zero;
            }
        }
    }
    [SerializeField] private int _directionInit;

    private Animator _animator;

    private float h { get => Input.GetAxisRaw("Horizontal"); }
    private float v { get => Input.GetAxisRaw("Vertical"); }

    private void Awake()
    {
        direction = _directionInit;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (h < 0f)
            direction = -1;
        if (h > 0f)
            direction = 1;

        _move.x = h;

        if (Mathf.Abs(_move.x) > 0f)
            ChangeState(State.Move);
        else
            ChangeState(State.Idle);

        UpdataState();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(_move.x * _moveSpeed, _move.y, 0)* Time.fixedDeltaTime;
    }

    private void UpdataState()
    {
        switch (state)
        {
            case State.Idle:
                UpDataIdleState();
                break;
            case State.Move:
                UpDataMoveState();
                break;
            case State.Jump:
                break;
            case State.Attack:
                break;
            case State.Dash:
                break;
            case State.Slide:
                break;
            default:
                break;
        }
    }

    private void ChangeState(State newState)
    {
        if (state == newState)
            return;

        // 이전 하위 상태머신 초기화
        switch (state)
        {
            case State.Idle:
                _idleState = IdleState.Idle;
                break;
            case State.Move:
                _moveState = MoveState.Idle;
                break;
            case State.Jump:
                break;
            case State.Attack:
                break;
            case State.Dash:
                break;
            case State.Slide:
                break;
            default:
                break;
        }
        // 다음 하위 상태머신 준비
        switch (newState)
        {
            case State.Idle:
                _idleState = IdleState.Prepare;
                break;
            case State.Move:
                _moveState = MoveState.Prepare;
                break;
            case State.Jump:
                break;
            case State.Attack:
                break;
            case State.Dash:
                break;
            case State.Slide:
                break;
            default:
                break;
        }
        state = newState;
    }

    private void UpDataIdleState()
    {
        switch (_idleState)
        {
            case IdleState.Idle:
                break;
            case IdleState.Prepare:
                _animator.Play("Idle");
                _idleState = IdleState.onAction;
                break;
            case IdleState.Casting:
                break;
            case IdleState.onAction:
                break;
            case IdleState.Finish:
                break;
            default:
                break;
        }
    }
    private void UpDataMoveState()
    {
        switch (_moveState)
        {
            case MoveState.Idle:
                break;
            case MoveState.Prepare:
                _animator.Play("Move");
                _moveState = MoveState.onAction;
                break;
            case MoveState.Casting:
                break;
            case MoveState.onAction:
                break;
            case MoveState.Finish:
                break;
            default:
                break;
        }
    }
}

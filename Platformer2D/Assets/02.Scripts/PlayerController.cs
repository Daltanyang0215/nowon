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
        Fall,
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
    private enum JumpState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }
    private enum FallState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }
    private enum SlideState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }

    public State state;
    [SerializeField] private IdleState _idleState;
    [SerializeField] private MoveState _moveState;
    [SerializeField] private JumpState _jumpState;
    [SerializeField] private FallState _fallState;
    [SerializeField] private SlideState _slideState;

    private Vector2 _move;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _jumpForce = 3.0f;
    [SerializeField] private float _slideSpeed = 2.0f;


    // -1 : left ,+1 : right
    private int _direction;
    public int direction
    {
        get { return _direction; }
        set
        {
            if (value < 0)
            {
                _direction = -1;
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
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
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private GroundDetector _groundDetector;

    private bool isMovable = true;
    private float _slideAnimationTime;
    private float _animationTimer;

    private float h { get => Input.GetAxisRaw("Horizontal"); }
    private float v { get => Input.GetAxisRaw("Vertical"); }


    private void Awake()
    {
        direction = _directionInit;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _groundDetector = GetComponent<GroundDetector>();
        _slideAnimationTime = GetAnimationTime("Slide");
    }

    private void Update()
    {
        if (state != State.Slide)
        {
            if (h < 0f)
                direction = -1;
            if (h > 0f)
                direction = 1;
        }

        if (isMovable)
        {
            _move.x = h;

            if (Mathf.Abs(_move.x) > 0f)
                ChangeState(State.Move);
            else
                ChangeState(State.Idle);
        }
        if (state != State.Jump && state != State.Fall)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ChangeState(State.Jump);
            if (Input.GetKeyUp(KeyCode.LeftShift))
                ChangeState(State.Slide);
        }


        UpdataState();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(_move.x * _moveSpeed, _move.y, 0) * Time.fixedDeltaTime;
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
                UpDataJumpState();
                break;
            case State.Fall:
                UpDataFallState();
                break;
            case State.Attack:
                break;
            case State.Dash:
                break;
            case State.Slide:
                UpDataSlideState();
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
                _jumpState = JumpState.Idle;
                break;
            case State.Fall:
                _fallState = FallState.Idle;
                break;
            case State.Attack:
                break;
            case State.Dash:
                break;
            case State.Slide:
                _slideState = SlideState.Idle;
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
                _jumpState = JumpState.Prepare;
                break;
            case State.Fall:
                _fallState = FallState.Prepare;
                break;
            case State.Attack:
                break;
            case State.Dash:
                break;
            case State.Slide:
                _slideState = SlideState.Prepare;
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
                isMovable = true;
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
                isMovable = true;
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
    private void UpDataJumpState()
    {
        switch (_jumpState)
        {
            case JumpState.Idle:
                break;
            case JumpState.Prepare:
                isMovable = false;
                _animator.Play("Jump");
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                _jumpState++;
                break;
            case JumpState.Casting:
                if (_groundDetector.isDetected == false)
                    _jumpState++;
                break;
            case JumpState.onAction:
                if (_rb.velocity.y < 0)
                {
                    ChangeState(State.Fall);
                }
                else if (_rb.velocity.y < 1f)
                {
                    _animator.Play("JumpToFall");
                }
                break;
            case JumpState.Finish:
                break;
            default:
                break;
        }
    }
    private void UpDataFallState()
    {
        switch (_fallState)
        {
            case FallState.Idle:
                break;
            case FallState.Prepare:
                isMovable = false;
                _animator.Play("Fall");
                _fallState = FallState.onAction;
                break;
            case FallState.Casting:
                break;
            case FallState.onAction:
                if (_groundDetector.isDetected && _rb.velocity.y == 0)
                    ChangeState(State.Idle);
                break;
            case FallState.Finish:
                break;
            default:
                break;
        }
    }
    private void UpDataSlideState()
    {
        switch (_slideState)
        {
            case SlideState.Idle:
                break;
            case SlideState.Prepare:
                isMovable = false;
                _animator.Play("Slide");
                _animationTimer = _slideAnimationTime;
                _slideState++;
                break;
            case SlideState.Casting:
                if (_animationTimer < _slideAnimationTime * 3 / 4)
                    _slideState++;
                else
                {
                    _rb.velocity = Vector2.right * direction * _moveSpeed;
                }
                _animationTimer -= Time.deltaTime;
                break;
            case SlideState.onAction:
                if (_animationTimer < _slideAnimationTime * 1 / 4)
                    _slideState++;
                else
                {
                    _rb.velocity = Vector2.right * direction * _moveSpeed * _slideSpeed;
                }
                _animationTimer -= Time.deltaTime;
                break;
            case SlideState.Finish:
                if (_animationTimer < 0)
                {
                    ChangeState(State.Idle);
                }
                else
                {
                    _rb.velocity = Vector2.right * direction * _moveSpeed;
                }
                _animationTimer -= Time.deltaTime;
                break;
            default:
                break;
        }
    }

    private float GetAnimationTime(string clipName)
    {
        RuntimeAnimatorController rac = _animator.runtimeAnimatorController;
        for (int i = 0; i < rac.animationClips.Length; i++)
        {
            if (rac.animationClips[i].name == clipName)
            {
                return rac.animationClips[i].length;
            }
        }
        return -1.0f;
    }
}

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
    private enum AttackState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }
    private enum DashState
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
    [SerializeField] private AttackState _attackState;
    [SerializeField] private DashState _dashState;

    private Vector2 _move;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _jumpForce = 3.0f;
    [SerializeField] private float _slideSpeed = 2.0f;
    [SerializeField] private float _dashSpeed = 1.5f;

    [SerializeField] private Vector2 _attackHitCastCenter;
    [SerializeField] private Vector2 _attackHitCastSize;
    [SerializeField] private LayerMask _attackTargetLayer;


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
    private Vector2 _colOffsetOrigin;
    private Vector2 _colSizeOrigin;
    [SerializeField] private Vector2 _colOffsetCrouch = new Vector2(0, 0.055f);
    [SerializeField] private Vector2 _colSizeCrouch = new Vector2(0.11f, 0.11f);

    private bool isMovable = true;
    private bool isDirectionChangable = true;
    private float _slideAnimationTime;
    private float _attackAnimationTime;
    private float _dashAnimationTime;
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
        _attackAnimationTime = GetAnimationTime("Attack");
        _dashAnimationTime = GetAnimationTime("Dash");
        _colOffsetOrigin = _col.offset;
        _colSizeOrigin = _col.size;
    }

    private void Update()
    {
        if (isDirectionChangable)
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
        if (Input.GetKeyDown(KeyCode.Z) && (state == State.Idle || state == State.Move || state == State.Jump || state == State.Fall))
        {
            ChangeState(State.Attack);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && (state == State.Idle || state == State.Move || state == State.Jump || state == State.Fall))
        {
            ChangeState(State.Dash);
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
                UpDataAttackState();
                break;
            case State.Dash:
                UpDataDashState();
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
                _attackState = AttackState.Idle;
                break;
            case State.Dash:
                _dashState = DashState.Idle;
                break;
            case State.Slide:
                _slideState = SlideState.Idle;
                _col.offset = _colOffsetOrigin;
                _col.size = _colSizeOrigin;
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
                _attackState = AttackState.Prepare;
                break;
            case State.Dash:
                _dashState = DashState.Prepare;
                break;
            case State.Slide:
                _slideState = SlideState.Prepare;
                _col.offset = _colOffsetCrouch;
                _col.size = _colSizeCrouch;
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
                isDirectionChangable = true;
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
                isDirectionChangable = true;
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
                isDirectionChangable = true;
                _animator.Play("Jump");
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
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
                isDirectionChangable = true;
                _animator.Play("Fall");
                _fallState = FallState.onAction;
                break;
            case FallState.Casting:
                break;
            case FallState.onAction:
                if (_groundDetector.isDetected)
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
                isDirectionChangable = false;
                _animator.Play("Slide");
                _animationTimer = _slideAnimationTime;
                _slideState++;
                break;
            case SlideState.Casting:
                if (_animationTimer < _slideAnimationTime * 3f / 4f)
                    _slideState++;
                else
                {
                    _rb.velocity = Vector2.right * direction * _moveSpeed;
                }
                _animationTimer -= Time.deltaTime;
                break;
            case SlideState.onAction:
                if (_animationTimer < _slideAnimationTime * 1f / 4f)
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
    private void UpDataDashState()
    {
        switch (_dashState)
        {
            case DashState.Idle:
                break;
            case DashState.Prepare:
                isMovable = false;
                isDirectionChangable = false;
                _animator.Play("Dash");
                _animationTimer = _dashAnimationTime;
                _dashState++;
                break;
            case DashState.Casting:
                if (_animationTimer < _dashAnimationTime * 3.5f / 4f)
                    _dashState++;
                else
                {
                    _rb.velocity = Vector2.right * direction * _moveSpeed;
                }
                _animationTimer -= Time.deltaTime;
                break;
            case DashState.onAction:
                if (_animationTimer < _dashAnimationTime * 1.5f / 4f)
                    _dashState++;
                else
                {
                    _rb.velocity = Vector2.right * direction * _moveSpeed * _dashSpeed;
                }
                _animationTimer -= Time.deltaTime;
                break;
            case DashState.Finish:
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

    private void UpDataAttackState()
    {
        switch (_attackState)
        {
            case AttackState.Idle:
                break;
            case AttackState.Prepare:
                isMovable = false;
                isDirectionChangable = false;
                _animator.Play("Attack");
                _attackState = AttackState.onAction;
                _animationTimer = _attackAnimationTime;
                break;
            case AttackState.Casting:
                break;
            case AttackState.onAction:
                if (_animationTimer < 0)
                {
                    ChangeState(State.Idle);
                }
                _animationTimer -= Time.deltaTime;
                break;
            case AttackState.Finish:
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

    private void AttackHit()
    {
        Vector2 attackCenter = new Vector2(_attackHitCastCenter.x * _direction, _attackHitCastCenter.y) + _rb.position;
        RaycastHit2D hit = Physics2D.BoxCast(_attackHitCastCenter, _attackHitCastSize, 0, Vector2.zero, 0, _attackTargetLayer);

        if (hit.collider != null)
        {
            Debug.Log($"Attack hit : {hit.collider.gameObject.name}");
        }
    }

}

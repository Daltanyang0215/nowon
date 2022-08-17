using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        Hurt,
        Die
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
    private enum AttackState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }
    private enum HurtState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }
    private enum DieState
    {
        Idle,
        Prepare,
        Casting,
        onAction,
        Finish
    }
    private enum AIState
    {
        Idle,
        DecideRandomBehavior,
        TakeAReset,
        MoveLeft,
        MoveRight,
        FollowTarget,
        AttactTarget,
    }

    [Header("States")]
    public State state;
    [SerializeField] private IdleState _idleState;
    [SerializeField] private MoveState _moveState;
    [SerializeField] private AttackState _attackState;
    [SerializeField] private HurtState _hurtState;
    [SerializeField] private DieState _dieState;
    [SerializeField] private AIState _aiState;

    [Header("AI")]
    [SerializeField] private bool _aiAutoFollow;
    [SerializeField] private bool _aiAttackable;
    [SerializeField] private float _aiTargetDetectRange;
    [SerializeField] private float _aiBehaviorTimeMin;
    [SerializeField] private float _aiBehaviorTimeMax;
    private float _aiBehaviorTime;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    private Vector2 _move;
    private int _direction;
    public int direction
    {
        get { return _direction; }
        set
        {
            if (value < 0)
            {
                _direction = -1;
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                _direction = 1;
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
    }
    [SerializeField] private int _directionInit = 1;

    [SerializeField] private LayerMask _targetLayer;

    private Animator _animator;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;

    private float _animationTimer;
    private float _AttackTime;
    private float _hurtTime;
    private float _dieTime;

    private bool _isMovable = true;
    private bool _isDirectionChangable = true;

    private void Awake()
    {
        direction = _directionInit;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _AttackTime = GetAnimationTime("Attack");
        _hurtTime = GetAnimationTime("Hurt");
        //_dieTime = GetAnimationTime("Die");
    }

    private void Update()
    {
        UpdateAIState();

        if (_isDirectionChangable)
        {
            if (_move.x < 0f)
                direction = -1;
            if (_move.x > 0f)
                direction = 1;
        }

        if (_isMovable)
        {
            if (Mathf.Abs(_move.x) > 0f)
                ChangeState(State.Move);
            else
                ChangeState(State.Idle);
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
                UpdateIdleState();
                break;
            case State.Move:
                UpdateMoveState();
                break;
            case State.Attack:
                UpdateAttackState();
                break;
            case State.Hurt:
                UpdateHurtState();
                break;
            case State.Die:
                UpdateDieState();
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
            case State.Attack:
                _attackState = AttackState.Idle;
                break;
            case State.Hurt:
                _hurtState = HurtState.Idle;
                break;
            case State.Die:
                _dieState = DieState.Idle;
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
            case State.Attack:
                _attackState = AttackState.Prepare;
                break;
            case State.Hurt:
                _hurtState = HurtState.Prepare;
                break;
            case State.Die:
                _dieState = DieState.Prepare;
                break;
            default:
                break;
        }
        state = newState;
    }

    private void UpdateAIState()
    {
        if (_aiAutoFollow)
        {
            if (Physics2D.OverlapCircle(_rb.position, _aiTargetDetectRange, _targetLayer))
            {
                _aiState = AIState.FollowTarget;
            }
        }
        else
        {
            // check player hit
        }

        switch (_aiState)
        {
            case AIState.Idle:
                _aiState++;
                break;
            case AIState.DecideRandomBehavior:
                _move.x = 0;
                _aiBehaviorTime = Random.Range(_aiBehaviorTimeMin, _aiBehaviorTimeMax);
                _aiState = (AIState)Random.Range((int)AIState.TakeAReset, (int)AIState.MoveRight + 1);
                break;
            case AIState.TakeAReset:
                if (_aiBehaviorTime < 0)
                {
                    _aiState = AIState.DecideRandomBehavior;
                }
                _aiBehaviorTime -= Time.deltaTime;
                break;
            case AIState.MoveLeft:
                if (_aiBehaviorTime < 0)
                {
                    _aiState = AIState.DecideRandomBehavior;
                }
                else
                {
                    _move.x = -1f;
                    _aiBehaviorTime -= Time.deltaTime;
                }
                break;
            case AIState.MoveRight:
                if (_aiBehaviorTime < 0)
                {
                    _aiState = AIState.DecideRandomBehavior;
                }
                else
                {
                    _move.x = 1f;
                    _aiBehaviorTime -= Time.deltaTime;
                }
                break;
            case AIState.FollowTarget:
                Collider2D target = Physics2D.OverlapCircle(_rb.position, _aiTargetDetectRange, _targetLayer);
                if (target == null)
                {
                    _aiState = AIState.DecideRandomBehavior;
                }
                else
                {
                    if (target.transform.position.x > _rb.position.x + _col.size.x)
                    {
                        _move.x = 1f;
                    }
                    else if (target.transform.position.x < _rb.position.x - _col.size.x)
                    {
                        _move.x = -1f;
                    }
                }
                break;
            case AIState.AttactTarget:
                break;
            default:
                break;
        }
    }

    private void UpdateIdleState()
    {
        switch (_idleState)
        {
            case IdleState.Idle:
                break;
            case IdleState.Prepare:
                //isMovable = true;
                //isDirectionChangable = true;
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
    private void UpdateMoveState()
    {
        switch (_moveState)
        {
            case MoveState.Idle:
                break;
            case MoveState.Prepare:
                //isMovable = true;
                //isDirectionChangable = true;
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

    private void UpdateAttackState()
    {
        switch (_attackState)
        {
            case AttackState.Idle:
                break;
            case AttackState.Prepare:
                //isMovable = false;
                //isDirectionChangable = false;
                _animator.Play("Attack");
                _attackState = AttackState.onAction;
                _animationTimer = _AttackTime;
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

    private void UpdateHurtState()
    {
        switch (_hurtState)
        {
            case HurtState.Idle:
                break;
            case HurtState.Prepare:
                break;
            case HurtState.Casting:
                break;
            case HurtState.onAction:
                break;
            case HurtState.Finish:
                break;
            default:
                break;
        }
    }
    private void UpdateDieState()
    {
        switch (_dieState)
        {
            case DieState.Idle:
                break;
            case DieState.Prepare:
                break;
            case DieState.Casting:
                break;
            case DieState.onAction:
                break;
            case DieState.Finish:
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _aiTargetDetectRange);
    }
}

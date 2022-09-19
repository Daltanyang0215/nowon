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
    [SerializeField] private float _aiAttackRange;
    [SerializeField] private float _aiBehaviorTimeMin;
    [SerializeField] private float _aiBehaviorTimeMax;
    private float _aiBehaviorTime;

    [Header("Movement")]
    [SerializeField] private bool _moveEnable;
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

    [SerializeField] protected LayerMask _targetLayer;

    private Animator _animator;
    protected Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    protected Enemy _enemySelf;

    private float _animationTimer;
    private float _AttackTime;
    private float _hurtTime;
    private float _dieTime;

    private bool _isMovable = true;
    private bool _isDirectionChangable = true;

    [SerializeField] private Vector2 _knockBackForce;

    public void TryHurt()
    {
        if (state == State.Hurt)
        {
            _animationTimer = _hurtTime;
        }
        else
        {
            ChangeState(State.Hurt);
        }
    }

    public void TryDie()
    {
        ChangeState(State.Die);
    }

    public void KnockBack(int knockbackDirection)
    {
        if (!_moveEnable) return;
        _move.x = 0;
        _rb.velocity = Vector2.zero;

        _rb.AddForce(new Vector2(knockbackDirection * _knockBackForce.x, _knockBackForce.y), ForceMode2D.Impulse);
    }

    protected virtual void AttackBehavior()
    {

    }

    private void Awake()
    {
        direction = _directionInit;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _enemySelf = GetComponent<Enemy>();
        _AttackTime = GetAnimationTime("Attack");
        _hurtTime = GetAnimationTime("Hurt");
        _dieTime = GetAnimationTime("Die");
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

        if (_moveEnable)
        {
            if (state != State.Hurt && state != State.Die)
            {
                if (_isMovable)
                {
                    if (Mathf.Abs(_move.x) > 0f)
                        ChangeState(State.Move);
                    else
                        ChangeState(State.Idle);
                }
            }
        }
        UpdataState();
    }

    private void FixedUpdate()
    {
        if (!_moveEnable) return;
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

        // ���� ���� ���¸ӽ� �ʱ�ȭ
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
        // ���� ���� ���¸ӽ� �غ�
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
        if (state == State.Hurt || state == State.Die) return;
        if (_aiState < AIState.FollowTarget)
        {
            if (_aiAutoFollow)
            {
                if (Physics2D.OverlapCircle(_rb.position, _aiTargetDetectRange, _targetLayer))
                {
                    _aiState = AIState.FollowTarget;
                }
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

                    if (_aiAttackable && Vector2.Distance(target.transform.position, _rb.position) < _aiAttackRange)
                    {
                        ChangeState(State.Attack);
                        _aiState = AIState.AttactTarget;
                    }
                }
                break;
            case AIState.AttactTarget:
                if (state != State.Attack)
                {
                    _aiState = AIState.FollowTarget;
                }
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
                _isMovable = true;
                _isDirectionChangable = true;
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
                _isMovable = true;
                _isDirectionChangable = true;
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
                _isMovable = false;
                _isDirectionChangable = false;
                _move.x = 0;
                _rb.velocity = Vector2.zero;
                _animator.Play("Attack");
                _attackState++;
                _animationTimer = _AttackTime;
                break;
            case AttackState.Casting:
                _attackState++;
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
                _isMovable = false;
                _isDirectionChangable = false;
                _animator.Play("Hurt");
                _animationTimer = _hurtTime;
                _hurtState = HurtState.onAction;
                break;
            case HurtState.Casting:
                break;
            case HurtState.onAction:
                if (_animationTimer < 0)
                {
                    ChangeState(State.Idle);
                }
                else
                {
                    _animationTimer -= Time.deltaTime;
                }
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
                _isMovable = false;
                _isDirectionChangable = false;
                _move.x = 0;
                _rb.velocity = Vector2.zero;
                _animator.Play("Die");
                _animationTimer = _dieTime;
                _dieState = DieState.onAction;
                break;
            case DieState.Casting:
                break;
            case DieState.onAction:
                if (_animationTimer < 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _animationTimer -= Time.deltaTime;
                }
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

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _aiTargetDetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _aiAttackRange);
    }
}
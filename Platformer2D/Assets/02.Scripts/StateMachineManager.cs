using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    //public bool isReady{ get => State.Idle; }
    public enum State
    {
        Idle,
        Move,
        Jump,
        Fall,
        Attack,
        Dash,
        Slide,
        Crouch,
        DownJump,
        Hurt,
        Die
    }
    public State state;
    private Dictionary<State, StateMachineBase> _machines = new Dictionary<State, StateMachineBase>();
    private Dictionary<KeyCode, State> _States = new Dictionary<KeyCode, State>();

    [SerializeField] private int _directionInit;
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

    public bool isDirectionChangable { get; set; }
    public bool isMovable { get; set; }
    private Vector2 _move;
    private float _moveSpeed = 2.0f;

    private AnimationManager _animationManager;
    private StateMachineBase _current;
    private Rigidbody2D _rb;
    private Player _player;

    [SerializeField] private Vector2 _attackHitCastCenter = new Vector2(0.25f, 0.25f);
    [SerializeField] private Vector2 _attackHitCastSize = new Vector2(0.50f, 0.50f);
    [SerializeField] private LayerMask _attackTargetLayer;

    private float h { get => Input.GetAxisRaw("Horizontal"); }
    private float v { get => Input.GetAxisRaw("Vertical"); }

    //==========================================================================================
    public void ResetVelocity()
    {
        _move.x = 0;
        _rb.velocity = Vector2.zero;
    }
    public void ChangeState(State newState)
    {
        if (state == newState && !_machines[newState].IsExecuteOk()) return;

        _machines[state].ForceStop();
        _machines[newState].Execute();
        _current = _machines[newState];
        state = newState;
    }

    //==========================================================================================

    private void Awake()
    {
        _animationManager = GetComponent<AnimationManager>();
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        //_machines.Add(State.Idle, new StateMachineIdle(State.Idle, this, _animationManager));
        //_machines.Add(State.Move, new StateMachineMove(State.Move, this, _animationManager));
        //_machines.Add(State.Jump, new StateMachineJump(State.Jump, this, _animationManager));
        //_States.Add(KeyCode.X, State.Jump);
        //_machines.Add(State.Fall, new StateMachineFall(State.Fall, this, _animationManager));
        //_machines.Add(State.Attack, new StateMachineAttack(State.Attack, this, _animationManager));
        //_States.Add(KeyCode.Z, State.Attack);


        InitStateMachines();

        _current = _machines[State.Idle];
        _current.Execute();
        //foreach (var machine in _machines.Values)
        //{
        //    if(machine.shortKey != KeyCode.None)
        //    {
        //        _States.Add(machine.shortKey,machine.)
        //    }
        //}
    }

    private void InitStateMachines()
    {
        Array values= Enum.GetValues(typeof(State));
        foreach (var value in values)
        {
            AddStateMachine((State)value);
        }
    }

    private void AddStateMachine(State addState)
    {
        if (_machines.ContainsKey(addState)) return;

        string typeName = "StateMachine" + addState.ToString();
        Type type = Type.GetType(typeName);
        if (type != null)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(State), typeof(StateMachineManager), typeof(AnimationManager) });
            StateMachineBase machine = constructorInfo.Invoke(new object[] { addState, this, _animationManager }) as StateMachineBase;

            _machines.Add(addState, machine);
            if (machine.shortKey != KeyCode.None)
            {
                _States.Add(machine.shortKey, addState);
            }

            Debug.Log($"{addState}의 머신이 등록되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{addState}의 머신이 없습니다.");
        }

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
        foreach (var shortKey in _States.Keys)
        {
            if (Input.GetKeyDown(shortKey))
            {
                ChangeState(_States[shortKey]);
                return;
            }
        }

        ChangeState(_current.UpdateState());
    }

    private void FixedUpdate()
    {
        _current.FixedUpdateState();
        transform.position += new Vector3(_move.x * _moveSpeed, _move.y, 0) * Time.fixedDeltaTime;
    }


    private void AttackHit()
    {
        Vector2 attackCenter = new Vector2(_attackHitCastCenter.x * _direction, _attackHitCastCenter.y) + _rb.position;
        RaycastHit2D hit = Physics2D.BoxCast(attackCenter, _attackHitCastSize, 0, Vector2.zero, 0, _attackTargetLayer);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.Hurt(_player.damage);
            }
            if (hit.collider.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.KnockBack(direction);
            }

        }
    }
}

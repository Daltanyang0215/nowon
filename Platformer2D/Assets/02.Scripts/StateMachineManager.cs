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

    private AnimationManager _animationManager;
    private StateMachineBase _current;

    private void Awake()
    {
        _animationManager = GetComponent<AnimationManager>();
        _machines.Add(State.Idle, new StateMachineIdle(State.Idle, this, _animationManager));
        _machines.Add(State.Idle, new StateMachineMove(State.Move, this, _animationManager));
        _machines.Add(State.Idle, new StateMachineJump(State.Jump, this, _animationManager));
        _machines.Add(State.Idle, new StateMachineFall(State.Fall, this, _animationManager));
    }

    private void Update()
    {
        ChangeState(_current.UpdateState());
    }

    private void FixedUpdate()
    {
        _current.FixedUpdateState();
    }

    private void ChangeState(State newState)
    {
        if (state == newState && !_machines[newState].IsExecuteOk()) return;

        _machines[state].ForceStop();
        _machines[newState].Execute();
        _current = _machines[newState];
        state = newState;
    }
}

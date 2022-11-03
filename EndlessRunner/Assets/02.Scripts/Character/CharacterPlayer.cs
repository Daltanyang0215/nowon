using System;
using System.Collections.Generic;
using Unity.Animations.SpringBones.GameObjectExtensions;
using UnityEngine;
[Flags]
public enum PlayerStateTypes
{
    Idle = 0 << 1,
    Move = 1 << 0,
    Jump = 1 << 1,
    Slide = 1 << 2,
    WallRun = 1 << 3,
    Hurt = 1 << 4,
    Die = 1 << 5,
    All = ~Idle
}
public class CharacterPlayer : CharacterBase
{

    private StateMachineBase<PlayerStateTypes> _machine;

    [SerializeField] private PlayerStateTypes _currentType => _machine.currentType;
    [SerializeField] private IState<PlayerStateTypes>.Commands _commands => _machine.current.current;

    [Header("Detechors")]
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] public WallDetector _wallDetector_L;
    [SerializeField] public WallDetector _wallDetector_R;
    [Header("Movement")]
    [SerializeField] private Movement _movement;
    public void StartMove()
    {
        _machine.ChangeStaet(PlayerStateTypes.Move);
    }

    public void ChangeMachineState(PlayerStateTypes newstate)
    {
        _machine.ChangeStaet(newstate);
    }

    protected override void Awake()
    {
        base.Awake();
        _machine = new StateMachineBase<PlayerStateTypes>(gameObject,
                                                    GetStateExecuteConditionMask(),
                                                    GetStateTransitionPairs());

        RegisterAllKeyActions();
    }
    private void RegisterAllKeyActions()
    {
        InputHandler.RegisterKeyDownAction(InputHandler.SHORTCUT_PLAYER_JUMP,
                                           () => _machine.ChangeStaet(PlayerStateTypes.Jump));
        InputHandler.RegisterKeyDownAction(InputHandler.SHORTCUT_PLAYER_SLIDE,
                                           () => _machine.ChangeStaet(PlayerStateTypes.Slide));
        InputHandler.RegisterKeyDownAction(InputHandler.SHORTCUT_PLAYER_MOVE_LEFT,
                                   () =>
                                   {
                                       if (_groundDetector.isDetected == false && _wallDetector_L.isDetected)
                                       {
                                           _machine.ChangeStaet(PlayerStateTypes.WallRun);
                                       }
                                       else
                                       {
                                           _movement.doMoveLeft = true;
                                       }
                                   });
        InputHandler.RegisterKeyDownAction(InputHandler.SHORTCUT_PLAYER_MOVE_RIGHT,
                                   () =>
                                   {
                                       if (_groundDetector.isDetected == false && _wallDetector_R.isDetected)
                                       {
                                           _machine.ChangeStaet(PlayerStateTypes.WallRun);
                                       }
                                       else
                                       {
                                           _movement.doMoveRight = true;
                                       }
                                   });

    }

    private void Update()
    {
        Debug.Log($"{_currentType}, {_commands}");
        _machine.Update();
    }

    private Dictionary<PlayerStateTypes, PlayerStateTypes> GetStateExecuteConditionMask()
    {
        Dictionary<PlayerStateTypes, PlayerStateTypes> result = new Dictionary<PlayerStateTypes, PlayerStateTypes>();

        result.Add(PlayerStateTypes.Idle, PlayerStateTypes.All);
        result.Add(PlayerStateTypes.Move, PlayerStateTypes.All);
        result.Add(PlayerStateTypes.Jump, PlayerStateTypes.Idle | PlayerStateTypes.Move);
        result.Add(PlayerStateTypes.Slide, PlayerStateTypes.Idle | PlayerStateTypes.Move);
        result.Add(PlayerStateTypes.WallRun, PlayerStateTypes.Jump);
        result.Add(PlayerStateTypes.Hurt, PlayerStateTypes.All);
        result.Add(PlayerStateTypes.Die, PlayerStateTypes.All);

        return result;
    }

    private Dictionary<PlayerStateTypes, PlayerStateTypes> GetStateTransitionPairs()
    {
        Dictionary<PlayerStateTypes, PlayerStateTypes> result = new Dictionary<PlayerStateTypes, PlayerStateTypes>();

        result.Add(PlayerStateTypes.Idle, PlayerStateTypes.Idle);
        result.Add(PlayerStateTypes.Move, PlayerStateTypes.Move);
        result.Add(PlayerStateTypes.Jump, PlayerStateTypes.Move);
        result.Add(PlayerStateTypes.Slide, PlayerStateTypes.Move);
        result.Add(PlayerStateTypes.WallRun, PlayerStateTypes.Move);
        result.Add(PlayerStateTypes.Hurt, PlayerStateTypes.Move);
        result.Add(PlayerStateTypes.Die, PlayerStateTypes.Move);

        return result;
    }
}


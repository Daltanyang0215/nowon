using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class CharacterEnemy : CharacterBase
{
    [Flags]
    public enum EnemyStateTypes
    {
        Idle = 0 << 1,
        Move = 1 << 0,
        Jump = 1 << 1,
        Attack = 1 << 2,
        Hurt = 1 << 4,
        Die = 1 << 5,
        All = ~Idle
    }

    private StateMachineBase<EnemyStateTypes> _machine;

    [SerializeField] private EnemyStateTypes _currentType => _machine.currentType;
    [SerializeField] private IState<EnemyStateTypes>.Commands _commands => _machine.current.current;

    [Header("Detechors")]
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _detectRange;
    [SerializeField] private float _detectAttackRange;

    public class BehaviorTreeForEnemy : BehaviorTree
    {
        private CharacterEnemy _owner;

        public class Detect : Execution
        {
            private Collider[] _tmp;
            public Detect(Func<ReturnType> function, Vector3 center, float detectRange, LayerMask targetLayer) : base(function)
            {
                function += () =>
                {
                    _tmp = Physics.OverlapSphere(center, detectRange, targetLayer);
                    if (_tmp != null &&
                        _tmp.Length > 0)
                    {
                        return ReturnType.Success;
                    }
                    return ReturnType.Failure;
                };
            }
        }

        public class Look : Execution
        {
            public Look(Func<ReturnType> function, Transform onwer, Transform target) : base(function)
            {
                function += () =>
                {
                    if (target == null)
                        return ReturnType.Failure;

                    onwer.LookAt(target);
                    return ReturnType.Success;
                };
            }
        }

        public override RootNode root { get; set; }
        public Selector selectorForTarget;
        public Sequence sequenceWhenTargetDetected;
        public ConditionNode conditionPlayerDetected;
        public ConditionNode conditionMovable;
        public RandomSelector randomSelectorForMoveMent;
        public Detect executionDetectPlayer;
        public Look executionLookPlayer;
        public Detect executionDetectPlayerInAttackRange;
        public Execution executionAttack;

        public BehaviorTreeForEnemy(CharacterEnemy owner)
        {
            _owner = owner;
        }

        public override void Init()
        {
            root = new RootNode();

            executionDetectPlayer = new Detect(null, _owner.transform.position, _owner._detectRange, _owner._targetLayer);
            executionDetectPlayerInAttackRange = new Detect(null, _owner.transform.position, _owner._detectAttackRange, _owner._targetLayer);
            //executionLookPlayer = new Look(null, _owner.transform);
        }

        public override ReturnType Tick()
        {
            return root.Invoke();
        }
    }
    private BehaviorTreeForEnemy _aiTree;

    public void ChangeMachineState(EnemyStateTypes newStateType) => _machine.ChangeStaet(newStateType);

    private void Awake()
    {
        _machine = new StateMachineBase<EnemyStateTypes>(gameObject,
                                                         GetStateExecuteConditionMask(),
                                                         GetStateTransitionParis());
        _aiTree = new BehaviorTreeForEnemy(this);
    }

    private void Update()
    {
        _aiTree.Tick();
        _machine.Update();
    }

    private Dictionary<EnemyStateTypes, EnemyStateTypes> GetStateExecuteConditionMask()
    {
        Dictionary<EnemyStateTypes, EnemyStateTypes> result = new Dictionary<EnemyStateTypes, EnemyStateTypes>();

        result.Add(EnemyStateTypes.Idle, EnemyStateTypes.All);
        result.Add(EnemyStateTypes.Move, EnemyStateTypes.All);
        result.Add(EnemyStateTypes.Jump, EnemyStateTypes.Idle | EnemyStateTypes.Move);
        result.Add(EnemyStateTypes.Attack, EnemyStateTypes.Idle | EnemyStateTypes.Move);
        result.Add(EnemyStateTypes.Hurt, EnemyStateTypes.All);
        result.Add(EnemyStateTypes.Die, EnemyStateTypes.All);

        return result;
    }

    private Dictionary<EnemyStateTypes, EnemyStateTypes> GetStateTransitionParis()
    {
        Dictionary<EnemyStateTypes, EnemyStateTypes> result = new Dictionary<EnemyStateTypes, EnemyStateTypes>();

        result.Add(EnemyStateTypes.Idle, EnemyStateTypes.Idle);
        result.Add(EnemyStateTypes.Move, EnemyStateTypes.Move);
        result.Add(EnemyStateTypes.Jump, EnemyStateTypes.Move);
        result.Add(EnemyStateTypes.Attack, EnemyStateTypes.Move);
        result.Add(EnemyStateTypes.Hurt, EnemyStateTypes.Move);
        result.Add(EnemyStateTypes.Die, EnemyStateTypes.Move);

        return result;
    }
}

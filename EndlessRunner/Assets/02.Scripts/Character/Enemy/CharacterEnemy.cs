using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using Unity.VisualScripting;

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
    public GroundDetector groundDetector;
    public LayerMask targetLayer;
    public GameObject target;
    public float detectRange;
    public float detectAttackRange;
    public bool movable;
    public Vector3 direction
    {
        get
        {
            return transform.eulerAngles;
        }
        set
        {
            transform.eulerAngles = value;
        }
    }

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
        public ConditionNode conditionPlayerNotDetected;
        public ConditionNode conditionMovable;
        public RandomSelector randomSelectorForMoveMent;
        public Execution executionDetectPlayer;
        public Execution executionLookPlayer;
        public Execution executionDetectPlayerInAttackRange;
        public Execution executionAttack;
        public Execution executionJumpForward;
        public Execution executionJumpBackward;

        public bool attackable;

        public BehaviorTreeForEnemy(CharacterEnemy owner)
        {
            _owner = owner;
        }

        public override void Init()
        {
            root = new RootNode();

            executionDetectPlayer = new Execution(() =>
            {
                Collider[] cols = Physics.OverlapSphere(_owner.transform.position, _owner.detectRange, _owner.targetLayer);
                if (cols.Length > 0)
                {
                    _owner.target = cols[0].gameObject;
                    return ReturnType.Success;
                }
                    _owner.target = null;
                return ReturnType.Failure;
            });
            executionLookPlayer = new Execution(() =>
            {
                if (_owner.target)
                {
                    _owner.transform.LookAt(_owner.target.transform);
                    return ReturnType.Success;
                }
                return ReturnType.Failure;

            });
            executionDetectPlayerInAttackRange = new Execution(() =>
            {
                Collider[] cols = Physics.OverlapSphere(_owner.transform.position, _owner.detectAttackRange, _owner.targetLayer);
                if (cols.Length > 0)
                {
                    _owner.target = cols[0].gameObject;
                    attackable = true;
                    return ReturnType.Success;
                }
                attackable = false;
                return ReturnType.Failure;
            });
            executionAttack = new Execution(() =>
            {
                if (attackable)
                {
                    if (_owner._machine.currentType == EnemyStateTypes.Attack)
                    {
                        if (_owner._machine.current.isBusy)
                            return ReturnType.OnRunning;
                        else
                            return ReturnType.Success;
                    }
                    _owner.ChangeMachineState(EnemyStateTypes.Attack);
                    if (_owner._machine.currentType == EnemyStateTypes.Attack)
                        return ReturnType.OnRunning;
                }
                return ReturnType.Failure;
            });

            sequenceWhenTargetDetected = new Sequence();
            sequenceWhenTargetDetected.Addchild(executionDetectPlayer)
                                      .Addchild(executionLookPlayer)
                                      .Addchild(executionDetectPlayerInAttackRange)
                                      .Addchild(executionAttack);
            executionJumpBackward = new Execution(() =>
            {
                if (_owner.groundDetector.isDetected)
                {
                    _owner.direction = Vector3.up * 180;
                    _owner.rb.velocity = Vector3.zero;
                    _owner.rb.AddRelativeForce(new Vector3(0.0f, 1.0f, 1.0f)*_owner.jumpForce, ForceMode.Impulse);
                    return ReturnType.Success;
                }
                return ReturnType.Failure;
            });
            executionJumpForward = new Execution(() =>
            {
                if (_owner.groundDetector.isDetected)
                {
                    _owner.direction = Vector3.up * 0;
                    _owner.rb.velocity = Vector3.zero;
                    _owner.rb.AddRelativeForce(new Vector3(0.0f, 1.0f, 1.0f)*_owner.jumpForce, ForceMode.Impulse);
                    return ReturnType.Success;
                }
                return ReturnType.Failure;
            });
            randomSelectorForMoveMent = new RandomSelector();
            randomSelectorForMoveMent.Addchild(executionJumpForward).Addchild(executionJumpBackward);
            conditionMovable = new ConditionNode(() => _owner.movable);
            conditionMovable.SetChild(randomSelectorForMoveMent);
            conditionPlayerNotDetected = new ConditionNode(() => !_owner.target);
            conditionPlayerNotDetected.SetChild(conditionMovable);
            selectorForTarget = new Selector();
            selectorForTarget.Addchild(sequenceWhenTargetDetected).Addchild(conditionPlayerNotDetected);
            root.SetChild(selectorForTarget);
        }

        public override ReturnType Tick()
        {
            Node dummy = null;
            ReturnType result;
            if (root.RunningNode != null)
            {
                result = root.RunningNode.Invoke(out dummy);
            }
            else
            {
                result = root.Invoke(out dummy);
            }
            return result;
        }
    }
    private BehaviorTreeForEnemy _aiTree;

    public void ChangeMachineState(EnemyStateTypes newStateType) => _machine.ChangeStaet(newStateType);

    protected override void Awake()
    {
        base.Awake();
        _machine = new StateMachineBase<EnemyStateTypes>(gameObject,
                                                         GetStateExecuteConditionMask(),
                                                         GetStateTransitionParis());
        _aiTree = new BehaviorTreeForEnemy(this);
    }
    private void Start()
    {
        _aiTree.Init();
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectAttackRange);
    }
}

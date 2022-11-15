using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Idle,
    Move,
    Jump,
    Attack,
    Hurt,
    Die
}

public class EnemyStateMachine : StateMachineBase<EnemyStates>
{
    public EnemyStateMachine(GameObject owner) : base(owner)
    {

    }

    protected override void InitStates()
    {
        IState<EnemyStates> tmp;
        GroundDetector groundDetector = owner.GetComponent<GroundDetector>();
        AnimationManager animationManager = owner.GetComponent<AnimationManager>();
        tmp = new EnemyStateIdle(EnemyStates.Idle,
                                 () => true,
                                 new List<KeyValuePair<System.Func<bool>, EnemyStates>>(),
                                 owner);
        states.Add(EnemyStates.Idle, tmp);

        tmp = new EnemyStateMove(EnemyStates.Move,
                                 () => groundDetector.IsDetected,
                                 new List<KeyValuePair<System.Func<bool>, EnemyStates>>(),
                                 owner);
        states.Add(EnemyStates.Move, tmp);

        tmp = new EnemyStateJump(EnemyStates.Jump,
                                 () => groundDetector.IsDetected &&
                                 (currntType == EnemyStates.Idle || currntType == EnemyStates.Move),
                                 new List<KeyValuePair<System.Func<bool>, EnemyStates>>()
                                 {
                                     new KeyValuePair<System.Func<bool>, EnemyStates>(()=> groundDetector.IsDetected,EnemyStates.Move)
                                 },
                                 owner);
        states.Add(EnemyStates.Jump, tmp);

        tmp = new EnemyStateAttack(EnemyStates.Attack,
                                 () => (currntType == EnemyStates.Idle || currntType == EnemyStates.Move),
                                 new List<KeyValuePair<System.Func<bool>, EnemyStates>>()
                                 {
                                     new KeyValuePair<System.Func<bool>, EnemyStates>(()=> animationManager.GetNormalizedTime() >= 1f,EnemyStates.Move)
                                 },
                                 owner);
        states.Add(EnemyStates.Attack, tmp);

        tmp = new EnemyStateHurt(EnemyStates.Hurt,
                                 () => true,
                                 new List<KeyValuePair<System.Func<bool>, EnemyStates>>()
                                 {
                                     new KeyValuePair<System.Func<bool>, EnemyStates>(()=> animationManager.GetNormalizedTime() >= 1f,EnemyStates.Move)
                                 },
                                 owner);
        states.Add(EnemyStates.Hurt, tmp);

        tmp = new EnemyStateDie(EnemyStates.Die,
                                 () => true,
                                 new List<KeyValuePair<System.Func<bool>, EnemyStates>>(),
                                 owner);
        states.Add(EnemyStates.Die, tmp);


    }
}

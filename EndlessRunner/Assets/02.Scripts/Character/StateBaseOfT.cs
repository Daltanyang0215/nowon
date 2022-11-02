using System;
using UnityEngine;

public abstract class StateBase<T> : IState<T> where T : Enum
{
    protected AnimationManager animationManager;
    protected StateMachineBase<T> stateMachine;
    protected T canExecuteConditionMask;
    protected T nextTargets;

    public bool isBusy => current > IState<T>.Commands.Idle && current < IState<T>.Commands.Finish;

    // 현재 상태
    public IState<T>.Commands current { get; protected set; }

    public virtual bool canExecute => canExecuteConditionMask.HasFlag(stateMachine.currentType) &&
                                      animationManager.isPreviousStateHasFinished;

    public T machineState { get; private set; }

    public StateBase(StateMachineBase<T> stateMachine,
        T machineState,
        T canExecuteCounditionMask,
        T nextTarget)
    {
        this.stateMachine = stateMachine;
        this.machineState = machineState;
        this.canExecuteConditionMask = canExecuteCounditionMask;
        this.nextTargets = nextTarget;
        animationManager = stateMachine.owner.GetComponent<AnimationManager>();
    }

    public virtual void Execute()
    {
        current = IState<T>.Commands.Prepare;
    }

    public virtual T Update()
    {
        T next = machineState;
        switch (current)
        {
            case IState<T>.Commands.Idle:
                break;
            case IState<T>.Commands.Prepare:
                MoveNext();
                break;
            case IState<T>.Commands.Casting:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForCastingFinished:
                MoveNext();
                break;
            case IState<T>.Commands.Action:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForActionFinished:
                MoveNext();
                break;
            case IState<T>.Commands.Finish:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForFinished:
                next = nextTargets;
                break;
            default:
                break;
        }
        return next;
    }

    public void MoveNext()
    {
        if (current >= IState<T>.Commands.WaitForFinished)
            Debug.LogWarning($"{this.GetType()} : not move next");
        else
        {
            current++;
        }
    }

    public virtual void Reset()
    {
        current = IState<T>.Commands.Idle;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class StateBase<T> : IState<T> where T : Enum
{
    public T stateType { get; protected set; }

    public bool canExecute => (_condition != null ? _condition.Invoke() : true)
                              && _animationManager.isPreviousStateMachineHasFinished
                              && _animationManager.isPreviousStateHasFinished;

    private Func<bool> _condition;
    private List<KeyValuePair<Func<bool>, T>> _transition;
    private AnimationManager _animationManager;

    public StateBase(T stateType, Func<bool> condition, List<KeyValuePair<Func<bool>, T>> transition, GameObject owner)
    {
        this.stateType = stateType;
        _condition = condition;
        _transition = transition;
        _animationManager = owner.GetComponent<AnimationManager>();
    }

    public void Execute()
    {
        Workflow().Reset();
    }
    public void Stop()
    {
        Workflow().Reset();
    }

    public T Tick()
    {
        return Workflow().Current;
    }
    public IEnumerator<T> Workflow()
    {
        
        while (true)
        {
            foreach (var transition in _transition)
            {
                if (transition.Key.Invoke())
                    yield return transition.Value;
            }
            yield return stateType;
        }
    }
}

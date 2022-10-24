using System;

public class StateDie<T> : StateBase<T> where T : Enum
{
    public StateDie(StateMachineBase<T> stateMachine, T machineState, T canExecuteCounditionMask, T nextTarget) : base(stateMachine, machineState, canExecuteCounditionMask, nextTarget)
    {
    }
}

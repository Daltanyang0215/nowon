using System;

public class StateSlide<T> : StateBase<T> where T : Enum
{
    public StateSlide(StateMachineBase<T> stateMachine, T machineState, T canExecuteCounditionMask, T nextTarget) : base(stateMachine, machineState, canExecuteCounditionMask, nextTarget)
    {
    }
}

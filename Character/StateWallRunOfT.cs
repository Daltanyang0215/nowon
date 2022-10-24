using System;

public class StateWallRun<T> : StateBase<T> where T : Enum
{
    public StateWallRun(StateMachineBase<T> stateMachine, T machineState, T canExecuteCounditionMask, T nextTarget) : base(stateMachine, machineState, canExecuteCounditionMask, nextTarget)
    {
    }
}

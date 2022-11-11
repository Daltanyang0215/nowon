using System;

public interface IState<T> : IState where T : Enum
{
    public T machineState { get; }
    new public T Update();
}


public interface IState
{
    public enum Commands
    {
        Idle,
        Prepare,
        Casting,
        WaitForCastingFinished,
        Action,
        WaitForActionFinished,
        Finish,
        WaitForFinished
    }
    public bool isBusy { get; }
    public Commands current { get; }
    public bool canExecute { get; }
    public void Execute();
    public void Reset();
    public object Update();

    public void MoveNext();

}

using System;

public class StateAttack<T> : StateBase<T> where T : Enum
{
    private CharacterBase _character;
    public StateAttack(StateMachineBase<T> stateMachine, T machineState, T canExecuteCounditionMask, T nextTarget) : base(stateMachine, machineState, canExecuteCounditionMask, nextTarget)
    {
        _character = stateMachine.owner.GetComponent<CharacterBase>();
    }

    public override bool canExecute => base.canExecute
    && _character.target;

    public override void Execute()
    {
        base.Execute();
        animationManager.SetBool("DoAttack", true);
        current = IState<T>.Commands.Casting;
    }
    public override void Reset()
    {
        base.Reset();
        animationManager.SetBool("DoAttack", false);
    }
    public override T Update()
    {
        T next = machineState;
        switch (current)
        {
            case IState<T>.Commands.Idle:
                MoveNext();
                break;
            case IState<T>.Commands.Prepare:
                MoveNext();
                break;
            case IState<T>.Commands.Casting:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForCastingFinished:
                if (animationManager.isCastingFinished)
                {
                    if (_character.target.TryGetComponent(out IHp target))
                    {
                        target.hp -= _character.atk;
                    }
                    animationManager.SetBool("DoAttack", false);
                    MoveNext();
                }
                break;
            case IState<T>.Commands.Action:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForActionFinished:
                if (animationManager.GetNormalizedTime() > 0.9f)
                    MoveNext();
                break;
            case IState<T>.Commands.Finish:
                next = nextTargets;
                break;
            case IState<T>.Commands.WaitForFinished:
                break;
            default:
                break;
        }
        return next;
    }
}

using System;
using Unity.Animations.SpringBones.GameObjectExtensions;
using UnityEngine;

public class StateWallRun<T> : StateBase<T> where T : Enum
{
    private Rigidbody _rb;
    private WallDetector wallDetector_L;
    private WallDetector wallDetector_R;
    public StateWallRun(StateMachineBase<T> stateMachine, T machineState, T canExecuteCounditionMask, T nextTarget) : base(stateMachine, machineState, canExecuteCounditionMask, nextTarget)
    {
        _rb = stateMachine.owner.GetComponent<Rigidbody>();
        wallDetector_L = stateMachine.owner.gameObject.FindChildByName("WallDetector_L").GetComponent<WallDetector>();
        wallDetector_R = stateMachine.owner.gameObject.FindChildByName("WallDetector_R").GetComponent<WallDetector>();
    }

    public override void Execute()
    {
        base.Execute();
        _rb.isKinematic = true;
    }

    public override T Update()
    {
        T next = machineState;
        switch (current)
        {
            case IState<T>.Commands.Idle:
                break;
            case IState<T>.Commands.Prepare:
                if (wallDetector_L.isDetected)
                {
                    animationManager.SetBool("DoWallRunLeft", true);
                }
                else if (wallDetector_R.isDetected)
                {
                    animationManager.SetBool("DoWallRunRight", true);
                }
                else
                    Debug.LogWarning("not wall detector");
                MoveNext();
                break;
            case IState<T>.Commands.Casting:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForCastingFinished:
                animationManager.SetBool("DoWallRunLeft", false);
                animationManager.SetBool("DoWallRunRight", false);
                MoveNext();
                break;
            case IState<T>.Commands.Action:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForActionFinished:
                if (animationManager.GetNormalizedTime() > 0.9f)
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

    public override void Reset()
    {
        base.Reset();
        _rb.isKinematic = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSlowingDown<T> : IBuff<T>
{
    private float _slowGain;

    public BuffSlowingDown(float slowGain)
    {
        _slowGain = slowGain;
    }

    public void OnActive(T target)
    {
        if (target is ISpeed)
        {
            ((ISpeed)target).Speed *= _slowGain;
        }
    }

    public void OnDeaction(T target)
    {
        if (target is ISpeed)
        {
            ((ISpeed)target).Speed = ((ISpeed)target).SpeedOrigin;
        }
    }

    public void OnDuration(T target)
    {
    }
}

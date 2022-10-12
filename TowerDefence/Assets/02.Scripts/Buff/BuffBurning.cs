using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBurning<T> : IBuff<T>
{
    private int _damage;
    private float _term;
    private float _timer;

    public BuffBurning(int damage, float term)
    {
        _damage = damage;
        _term = term;
        _timer = _term;
    }

    public void OnActive(T target)
    {
        
    }

    public void OnDeaction(T target)
    {
    }

    public void OnDuration(T target)
    {
        if (_timer < 0)
        {
            if (target is IHp)
            {
                ((IHp)target).HP -= _damage;
                _timer = 0;
            }
        }
        else
        {
            _timer-=Time.deltaTime;
        }
    }
}

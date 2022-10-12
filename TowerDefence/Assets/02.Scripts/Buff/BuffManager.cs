using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    private Dictionary<object, Dictionary<IBuff<object>, Coroutine>> _buffs = new Dictionary<object, Dictionary<IBuff<object>, Coroutine>>();

    private Coroutine _tmpCoroutine;



    public void ActiveBuff<T>(T target, IBuff<T> buff, float duration)
    {
        if (!_buffs.ContainsKey(target))
        {
            _buffs.Add(target, new Dictionary<IBuff<object>, Coroutine>());
        }
        
        if (_buffs[target].ContainsKey((IBuff<object>)buff))
        {
            StopCoroutine(_buffs[target][(IBuff<object>)buff]);
            _buffs[target].Remove((IBuff<object>)buff);
        }

        _tmpCoroutine = StartCoroutine(E_ActiveBuff(target, buff, duration));
        _buffs[target].Add((IBuff<object>)buff, _tmpCoroutine);
    }

    public void DeactiveAllBuffs<T>(T target)
    {
        if (!_buffs.ContainsKey(target)) return;

        foreach (Coroutine coroutine in _buffs[target].Values)
        {
            StopCoroutine(coroutine);
        }
        _buffs.Remove(target);
    }

    private IEnumerator E_ActiveBuff<T>(T target, IBuff<T> buff, float duration)
    {
        buff.OnActive(target);
        float timer = duration;

        while (timer > 0)
        {
            buff.OnDuration(target);
            timer -= Time.deltaTime;
            yield return null;
        }

        buff.OnDeaction(target);

        if (target != null)
        {
            _buffs[target][(IBuff<object>)buff] = null;
            _buffs[target].Remove((IBuff<object>)buff);
        }

    }

    private void Awake()
    {
        instance = this;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IHp
{
    private int _hp;
    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value < 0)
                value = 0;

            _hp = value;
            _hpBar.value = (float)_hp / hpMax;
            OnHPChanged?.Invoke(_hp);
            if (_hp <= 0)
                Die();
        }
    }

    public int hpMax;
    [SerializeField] private Slider _hpBar;

    public event Action OnDie;
    public event Action<int> OnHPChanged;

    public void Die()
    {
        OnDie();
        BuffManager.instance.DeactiveAllBuffs(this);
        ObjectPool.Instance.Return(gameObject);
    }

    private void Awake()
    {
        HP = hpMax;
    }

    public void DieEventClear()
    {
        OnDie = null;
    }
}

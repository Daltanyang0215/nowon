using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IHp,ISpeed
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

    private float _speed;
    public float Speed {
        get { return _speed; }
        set
        {
            _speed = value;
            OnSpeedChanged?.Invoke(_speed);
        }
    }

    private float _speedOrigin;
    public float SpeedOrigin
    {
        get
        {
            return _speedOrigin;
        }
        private set
        {
            _speedOrigin = value;
        }
    }

    public int hpMax;
    [SerializeField] private Slider _hpBar;

    public event Action OnDie;
    public event Action<int> OnHPChanged;
    public event Action<float> OnSpeedChanged;

    public BuffManager<Enemy> BuffManager;

    public void Die()
    {
        OnDie();
        ObjectPool.Instance.Return(gameObject);
    }

    private void Awake()
    {
        HP = hpMax;
        SpeedOrigin = 1.0f;
        Speed = 1.0f;
        BuffManager = new BuffManager<Enemy>(this);
    }

    public void DieEventClear()
    {
        OnDie = null;
    }

    private void OnDisable()
    {
        BuffManager.DeactiveAllBuffs();
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private int _hp;
    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value < 0) value = 0;

            _hp = value;
            _hpBar.value = (float)value / _hpMax;
        }
    }
    [SerializeField] private Slider _hpBar;
    [SerializeField] private int _hpMax;
    [SerializeField] private int _damage;


    private EnemyController _controller;

    [SerializeField] private LayerMask _targetlayer;

    public void Hurt(int damage)
    {
        hp -= damage;
        if (hp > 0)
            _controller.TryHurt();
        else
            _controller.TryDie();
    }


    private void Awake()
    {
        hp = _hpMax;
        _controller = GetComponent<EnemyController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if(go != null)
        {
            if (1<<go.layer == _targetlayer)
            {
                if(go.TryGetComponent(out Player player))
                {
                    if (player.invincible == false)
                    {
                        player.Hurt(_damage);
                        go.GetComponent<PlayerController>().KnockBack();
                    }
                }
            }
        }
    }

}

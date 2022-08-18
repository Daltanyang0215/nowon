using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool invincible { get; set; }
    private Coroutine _invincibleCoroutine;
    public int damage;

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
    private PlayerController _controller;

    public void Hurt(int damage)
    {
        hp -= damage;
        if (hp > 0)
            _controller.TryHurt();
        else
            _controller.TryDie();
    }

    public void InvincibleForSeconds(float seconds)
    {
        if(_invincibleCoroutine != null)
        {
            StopCoroutine(_invincibleCoroutine);
        }
        _invincibleCoroutine = StartCoroutine(E_invincibleForSeconds(seconds));
    }
    IEnumerator E_invincibleForSeconds(float seconds)
    {
        invincible = true;
        yield return new WaitForSeconds(seconds);
        _invincibleCoroutine = null;
        invincible = false;
    }

    private void Awake()
    {
        hp = _hpMax;
        _controller=GetComponent<PlayerController>();
    }
}

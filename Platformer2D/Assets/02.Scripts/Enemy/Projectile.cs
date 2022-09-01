using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _destroyEffect;
    private Vector2 _moveVec;
    private int _damage;
    private LayerMask _targetLayer;
    private float _elapsedTime;
    public delegate Vector2 CalcMoveVecHandler(float time);
    private event CalcMoveVecHandler CalcMoveVec;


    public void Setup(Vector2 direction, int damage, LayerMask targetLayer, CalcMoveVecHandler calcMethod)
    {
        _moveVec = direction;
        _damage = damage;
        _targetLayer = targetLayer;
        CalcMoveVec += calcMethod;
    }
    private void ShowEffect()
    {
        if (_destroyEffect == null) return;
        GameObject go = Instantiate(_destroyEffect.gameObject,transform.position,Quaternion.identity);
        Destroy(go,_destroyEffect.main.duration);
    }

    private void FixedUpdate()
    {
        if (CalcMoveVec != null)
        {
            _moveVec = CalcMoveVec(_elapsedTime);
        }

        transform.Translate(_moveVec * Time.fixedDeltaTime, Space.World);
        _elapsedTime += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == _targetLayer)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.Hurt(_damage);
                player.GetComponent<StateMachineManager>().KnockBack();
                ShowEffect();
                Destroy(gameObject);
            }
        }
        else if (1 << collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ShowEffect();
            Destroy(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : Projectile
{
    [SerializeField] private ParticleSystem _explosionEffect;

    protected override void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == targetLayer
             || 1 << other.gameObject.layer == touchLayer)
        {
            if (other.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.HP -= damage;
                //GameObject effect = Instantiate(_explosionEffect.gameObject, tr.position, Quaternion.LookRotation(tr.position - target.position));
                //Destroy(effect,_explosionEffect.main.duration + _explosionEffect.main.startLifetime.constantMax);
            }

                GameObject effect = ObjectPool.Instance.Spawn("Effect", tr.position, Quaternion.LookRotation(tr.position - other.transform.position));

                ObjectPool.Instance.Return(effect, _explosionEffect.main.duration + _explosionEffect.main.startLifetime.constantMax);
                ObjectPool.Instance.Return(gameObject);

        }
    }
}

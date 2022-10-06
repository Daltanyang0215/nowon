using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRocket : Projectile
{
    [SerializeField] private ParticleSystem _explosionEffect;
    private float _explosionRange=5;

    protected override void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == targetLayer
            || 1 << other.gameObject.layer == touchLayer        )
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosionRange, Vector3.up,0f,targetLayer);
            
            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].collider.gameObject.TryGetComponent(out Enemy enemy))
                {
                    //enemy.hp -=  (int)((_explosionRange - Vector3.Distance(transform.position,enemy.transform.position))* damage) ;
                    enemy.hp -=  damage ;
                    //GameObject effect = Instantiate(_explosionEffect.gameObject, tr.position, Quaternion.LookRotation(tr.position - target.position));
                    //Destroy(effect,_explosionEffect.main.duration + _explosionEffect.main.startLifetime.constantMax);

                }
            }

                    GameObject effect = ObjectPool.Instance.Spawn("RocketEffect", tr.position, Quaternion.LookRotation(tr.position - target.position));
                    ObjectPool.Instance.Return(effect, _explosionEffect.main.duration + _explosionEffect.main.startLifetime.constantMax);
                    ObjectPool.Instance.Return(gameObject);
        }
    }
}

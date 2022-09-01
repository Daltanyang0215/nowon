using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerForDarkNepenthes : EnemyController
{
    [Header("Projectile")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    protected override void AttackBehavior()
    {
        GameObject go = Instantiate(_projectilePrefab,_firePoint.position,Quaternion.identity);
        go.GetComponent<Projectile>().Setup(Vector2.right * direction, _enemySelf.damage, _targetLayer, null);
    }
}

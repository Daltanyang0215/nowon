using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerForNepenthes : EnemyController
{
    [SerializeField] private Vector2 _attackBoxCastCenter;
    [SerializeField] private Vector2 _attackBoxCastSize;

    protected override void AttackBehavior()
    {
        RaycastHit2D hit = Physics2D.BoxCast(origin    : _rb.position + new Vector2(_attackBoxCastCenter.x * direction, _attackBoxCastCenter.y),
                                             size      : _attackBoxCastSize,
                                             angle     : 0,
                                             direction : Vector2.zero,
                                             distance  : 0,
                                             layerMask : _targetLayer);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent(out Player player))
            {
                if (player.invincible == false)
                {
                    player.Hurt(_enemySelf.damage);
                    player.GetComponent<StateMachineManager>().KnockBack();
                }
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(transform.position + new Vector3(_attackBoxCastCenter.x * direction, _attackBoxCastCenter.y, 0), _attackBoxCastSize);

    }
}

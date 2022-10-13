using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TowerLaserLauncher : Tower
{
    [SerializeField] private LineRenderer _laserBeam;
    [SerializeField] private ParticleSystem _laserHitEffect;
    [SerializeField] private Transform _firePoint;

    [SerializeField] private int _damage;
    private int _damageStep
    {
        set
        {
            switch (value)
            {
                case 0:
                    _laserBeam.startWidth = 0.05f;
                    break;
                case 1:
                    _laserBeam.startWidth = 0.1f;
                    break;
                case 2:
                    _laserBeam.startWidth = 0.2f;
                    break;

                default:
                    break;
            }
        }
    }
    [SerializeField] private int _damageGain;
    [SerializeField] private float _damageChargeTime;
    private float _damageChargeTimer;
    private BuffSlowingDown<Enemy> _slowingDownBuff = new BuffSlowingDown<Enemy>(0.5f);
    private Enemy _enemy;

    protected override void Update()
    {
        base.Update();
        Attack();
    }

    private void Attack()
    {
        if (target == null)
        {
            _laserBeam.enabled = false;
            _laserHitEffect.Stop();
            if (_enemy != null && _enemy.BuffManager.IsBuffExist(_slowingDownBuff))
            {
                _enemy.BuffManager.DeactiveBuff(_slowingDownBuff);
            }
        }
        else
        {
            _laserBeam.SetPosition(0, _firePoint.position);
            _laserBeam.SetPosition(1, target.position);
            _laserBeam.enabled = true;

            if (_laserHitEffect.isStopped)
            {
                _laserHitEffect.Play();
            }
            RaycastHit[] hits = Physics.RaycastAll(_firePoint.position, target.position - _firePoint.position, _targetLayer);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.transform == target)
                {
                    _laserHitEffect.transform.position = hit.point;
                    _laserHitEffect.transform.LookAt(_firePoint);
                    break;
                }
            }

            _enemy = target.GetComponent<Enemy>();

            _enemy.HP -= _damage;
            if (_enemy.BuffManager.IsBuffExist(_slowingDownBuff))
                _enemy.BuffManager.ActiveBuff(_slowingDownBuff, 99999f);
        }
    }
}

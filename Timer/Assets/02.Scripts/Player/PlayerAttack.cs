using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool _attackCool = false;
    private bool AttackCool
    {
        get { return _attackCool; }
        set
        {
            _attackCool = value;
            PlayerMovement.instance.isAttack = _attackCool;
        }
    }

    [SerializeField] private float bulletCool;
    [SerializeField] private GameObject _bullet;

    [SerializeField] private LayerMask _targetMask;

    [SerializeField] private TimeLock _lockTarget;
    private bool _islock;

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(1)) // ¿ø°Å¸®
        {
            if (_bullet.activeSelf)
            {
                _bullet.transform.position = PlayerMovement.instance.model.position;
                _bullet.SetActive(false);
                if (_islock)
                {
                    _lockTarget.Lock(false);
                    _islock=false;
                }
            }
            else
            {
                AttackCool = true;
                BulletMovePoint(MouseClick());
                Invoke("UnAttack", bulletCool);
            }
        }
    }

    private void UnAttack()
    {
        AttackCool = false;
        //slash.SetActive(false);
    }

    private Vector3 MouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        Vector3 nextVec = Vector3.zero;
        if (Physics.Raycast(ray, out rayHit, 1000, _targetMask))
        {
            nextVec = rayHit.point - transform.position;
            Vector3 viewVec = new Vector3(nextVec.x, 0, nextVec.z);
            PlayerMovement.instance.moveVec = viewVec;

            if(rayHit.collider.gameObject.TryGetComponent<TimeLock>(out _lockTarget)) // rayhit check . component<timelock>
            {
                _islock = true;
                _lockTarget.Lock(true);
            }
        }
        return nextVec;
    }

    private void BulletMovePoint(Vector3 rayVec)
    {
        Ray ray = new Ray(transform.position, rayVec);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 1000, _targetMask))
        {
            _bullet.transform.position = PlayerMovement.instance.model.position;
            _bullet.transform.position = rayHit.point;
            _bullet.transform.rotation = Quaternion.LookRotation(rayVec.normalized);
            _bullet.gameObject.SetActive(true);
        }
    }
}

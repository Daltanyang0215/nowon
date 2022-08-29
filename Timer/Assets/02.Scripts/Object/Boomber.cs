 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomber : MonoBehaviour
{
    enum LockState
    {
        Idle,
        Freezing,
        Freeze,
        Melt
    }
    [SerializeField]
    private LockState _lockState = LockState.Idle;

    private TimeLock _timelock;
    [SerializeField]
    private float _boomtime;
    private float _time;

    private Rigidbody _rb;
    private Vector3 _prevVelocity;
    private Vector3 _prevRotate;

    public bool isLock
    {
        get { return _timelock != null ? _timelock.islock:false; }
    }

    private void Awake()
    {
        TryGetComponent<TimeLock>(out _timelock);
        _rb = GetComponent<Rigidbody>();
        _time = _boomtime;
    }
    private void FixedUpdate()
    {
        switch (_lockState)
        {
            case LockState.Idle:
                if (isLock)
                    _lockState++;
                break;
            case LockState.Freezing:
                _prevVelocity = _rb.velocity;
                _prevRotate = _rb.angularVelocity;

                _rb.constraints = RigidbodyConstraints.FreezeAll;

                _lockState++;
                break;
            case LockState.Freeze:
                if (!isLock)
                    _lockState++;

                break;
            case LockState.Melt:
                _rb.constraints = RigidbodyConstraints.None;

                _rb.velocity = _prevVelocity;
                _rb.angularVelocity= _prevRotate ;

                _lockState = LockState.Idle;
                break;
            default:
                break;
        } // �ð� ���� �� �ӵ��� ������Ʈ ���� �� ������ �ε�
    }

    private void Update()
    {
        if (!isLock)
        {
            if (_time < 0)
            {
                Boom();
            }
            else
            {
                _time -= Time.deltaTime;
            }
        }
    }


    void Boom()
    {
        Debug.Log("boom");
        gameObject.SetActive(false);
    }

}

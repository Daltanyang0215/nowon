using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    private void Awake()
    {
        instance = this;
        _rb = GetComponent<Rigidbody>();
    }

    public Transform model;

    [SerializeField] private float _moveSpeed;
    public float speedCorrection = 1.0f;
    private float _hAxis { get => Input.GetAxisRaw("Horizontal"); }
    private float _vAxis { get => Input.GetAxisRaw("Vertical"); }

    private Vector3 _moveVec;
    public Vector3 moveVec
    {
        get => _moveVec;
        set
        {
            _moveVec = value;
            model.transform.LookAt(model.transform.position + moveVec);
        }
    }
    private Vector3 nextVec;

    private Rigidbody _rb;

    public bool isAttack;

    private void FixedUpdate()
    {
        Move();
        //Turn();
    }

    void Move()
    {
        if (!isAttack)
        {
            moveVec = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(_hAxis, 0, _vAxis).normalized; ; // movevec 를 45도 회전 ( 카메라에 맞게 정렬)
        }


        if (!isAttack)
        {
            _rb.velocity = _moveSpeed * speedCorrection * moveVec;
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }

    //void Turn()
    //{
    //    model.transform.LookAt(model.transform.position + moveVec);
    //}

}

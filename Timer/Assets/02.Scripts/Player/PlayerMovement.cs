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

    public Vector3 moveVec;
    private Vector3 nextVec;

    private Rigidbody _rb;

    public bool isAttack;

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        if (!isAttack)
        {
            moveVec = new Vector3(_hAxis, 0, _vAxis).normalized;
            moveVec = Quaternion.AngleAxis(45, Vector3.up) * moveVec; // movevec �� 45�� ȸ�� ( ī�޶� �°� ����)
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

    void Turn()
    {
        model.transform.LookAt(model.transform.position + moveVec);
    }

}

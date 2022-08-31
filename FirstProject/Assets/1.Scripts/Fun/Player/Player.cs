using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    private void Awake()
    {
        Instance= this;
    }

    [Header("Move")]
    private float _forwad, _right;
    public float moveSpeed;
    private Vector3 _moveVec;

    private bool _isDash;
    private Vector3 _dashVec;

    void Update()
    {
        GetInput();
        //BoxDestroy();
        //BulletShot();
        Dash();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        _forwad = Input.GetAxisRaw("Vertical");
        _right = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        if (!_isDash)
            _moveVec = new Vector3(_right, 0, _forwad).normalized;
        else
            _moveVec = _dashVec;

        transform.Translate(_moveVec * moveSpeed * Time.fixedDeltaTime);
    }

    private void Dash()
    {
        if(_moveVec != Vector3.zero && !_isDash && Input.GetKeyDown(KeyCode.Space))
        {
            _isDash = true;
            moveSpeed *= 4;
            _dashVec = _moveVec;

            Invoke("Undash", 0.2f);
        }
    }

    private void Undash()
    {
        _isDash = false;
        moveSpeed /= 4;
    }
}

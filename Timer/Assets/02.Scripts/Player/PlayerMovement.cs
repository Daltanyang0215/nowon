using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public Transform model;

    // move
    [SerializeField] private float _moveSpeed;
    private float _hAxis { get => Input.GetAxisRaw("Horizontal"); }
    private float _vAxis { get => Input.GetAxisRaw("Vertical"); }
    [HideInInspector]
    public Vector3 moveVec;
    private Vector3 _nextVec;

    Rigidbody rigi;


    // dash
    private Vector3 _dashVec;
    [SerializeField] private float _dashPower;
    [SerializeField] private float _dashCoolTime;
    [SerializeField] private ParticleSystem dashParticale;
    private bool isDash = false;

    public bool isAttack = false;
    public bool IsMoveOK
    {
        get
        {
            return isAttack || isDash;
        }
    }

    private void Awake()
    {
        Instance = this;
        rigi = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Turn();
        Dash();
    }


    void GetInput()
    {
    }

    void Move()
    {
        if (!IsMoveOK)
        {
            moveVec = new Vector3(_hAxis, 0, _vAxis).normalized;
            moveVec = Quaternion.AngleAxis(45, Vector3.up) * moveVec; // movevec 를 45도 회전 ( 카메라에 맞게 정렬)
        }


        if (!isAttack)
        {
            transform.Translate(moveVec * _moveSpeed * Time.fixedDeltaTime); // 이동
            //rigi.velocity = moveVec * _moveSpeed;
        }

    }

    void Turn()
    {
        model.transform.LookAt(model.transform.position + moveVec);
    }

    void Dash()
    {
        if ((moveVec != Vector3.zero) && Input.GetKeyDown(KeyCode.Space) && !isDash)
        {
            isDash = true;
            _dashVec = moveVec;
            _moveSpeed *= _dashPower;
            dashParticale.Play();
            Invoke("UnDashing", _dashCoolTime);
        }
    }

    void UnDashing()
    {
        _moveSpeed /= _dashPower;
        isDash = false;
    }
}

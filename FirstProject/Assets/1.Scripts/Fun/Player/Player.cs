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
    private float forwad, right;
    public float moveSpeed;
    private Vector3 moveVec;

    private bool isDash;
    private Vector3 dashVec;

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
        forwad = Input.GetAxisRaw("Vertical");
        right = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        if (!isDash)
            moveVec = new Vector3(right, 0, forwad).normalized;
        else
            moveVec = dashVec;

        transform.Translate(moveVec * moveSpeed * Time.fixedDeltaTime);
    }

    private void Dash()
    {
        if(moveVec != Vector3.zero && !isDash && Input.GetKeyDown(KeyCode.Space))
        {
            isDash = true;
            moveSpeed *= 4;
            dashVec = moveVec;

            Invoke("Undash", 0.2f);
        }
    }

    private void Undash()
    {
        isDash = false;
        moveSpeed /= 4;
    }
}

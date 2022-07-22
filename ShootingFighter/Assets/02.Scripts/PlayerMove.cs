using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    

    private void FixedUpdate()
    {
        Move();
    }

    

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        Vector3 moveVec = dir * moveSpeed * Time.fixedDeltaTime;
        transform.Translate(moveVec);
    }

   
}

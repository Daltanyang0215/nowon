using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed =1;
    public float rotateSpeed;

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        Vector3 moveVec = dir * moveSpeed * Time.fixedDeltaTime;
        transform.Translate(moveVec);

        float r = Input.GetAxis("Mouse X");
        Vector3 rotateVec = new Vector3(0, r, 0) * rotateSpeed * Time.fixedDeltaTime;
        transform.Rotate(rotateVec);
    }

}

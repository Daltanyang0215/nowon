using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance;
    private void Awake()
    {
        if(Instance != null)
            Destroy(Instance);
        Instance= this;
    }

    [Header("Move")]
    public float moveSpeed;
    private Vector3 _moveVec;

    private bool _isDash;
    private Vector3 _dashVec;

    private void FixedUpdate()
    {
        transform.Translate(_moveVec * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        Dash();
    }


    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input != null)
        {
            if (!_isDash)
                _moveVec = new Vector3(input.x, 0, input.y).normalized;
            else
                _moveVec = _dashVec;
        }
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

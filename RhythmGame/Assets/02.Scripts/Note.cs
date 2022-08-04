using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public KeyCode keyCode;
    private Transform _tr;
    public float speed;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _tr.Translate(Vector2.down * speed * Time.fixedDeltaTime);
    }

}

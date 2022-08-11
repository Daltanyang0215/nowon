using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isDetected;
    private CapsuleCollider2D _col;
    private Vector2 _size;
    private Vector2 _center;
    [SerializeField] private LayerMask _targetLayer;
    private void Awake()
    {
        _col = GetComponent<CapsuleCollider2D>();
        _size.x = _col.size.x / 2;
        _size.y = 0.005f;
    }

    private void Update()
    {
        _center.x = transform.position.x - _col.offset.x;
        _center.y = transform.position.y + _col.offset.y - (_col.size.y + _size.y) / 2 - 0.01f;
        isDetected = Physics2D.OverlapBox(_center, _size, 0, _targetLayer);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_center, _size);
    }
}

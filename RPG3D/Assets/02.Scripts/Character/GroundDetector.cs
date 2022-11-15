using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool IsDetected;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _range;
    [SerializeField] private LayerMask _groundLayer;

    private void FixedUpdate()
    {
        IsDetected = Physics.CheckSphere(transform.position + _offset, _range, _groundLayer);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetector : MonoBehaviour
{
    public bool isDetected { get => _detectingRisingEdge || _detectingFallingEdge; }
    //public float topX, topY, bottomX, bottomY;
    public Vector2 climbPos
    {
        get => new Vector2(topX*1.1f, topY*1.05f);
    }
    public Vector2 grabPos
    {
        get => new Vector2(topX, topY * 1.05f);
    }
    public float topX => _rb.position.x + (_col.size.x / 2f + 0.04f) * _machineManager.direction;
    public float topY => _rb.position.y + _col.size.y * 1.1f + 0.03f;
    public float bottomX => _rb.position.x + (_col.size.x / 2f + 0.04f) * _machineManager.direction;
    public float bottomY => _rb.position.y + _col.size.y * 1.1f - 0.03f;
    public bool topOn, bottomOn;
    private bool _detectingFallingEdge;
    private bool _detectingRisingEdge;
    private StateMachineManager _machineManager;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    [SerializeField] LayerMask _groundLayer;

    private void Awake()
    {
        _machineManager = GetComponent<StateMachineManager>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        topOn = Physics2D.OverlapCircle(new Vector2(topX, topY),
                                                    0.01f,
                                                    _groundLayer);

        bottomOn = Physics2D.OverlapCircle(new Vector2(bottomX , bottomY),
                                                       0.01f,
                                                       _groundLayer);
        // ¶³¾îÁö´Â Áß
        if (bottomOn && !topOn && _rb.velocity.y < 0)
        {
            _detectingRisingEdge = true;
        }
        else
        {
            _detectingRisingEdge = false;
        }

        //// »ó½ÂÇÏ´Â Áß
        //if (!bottomOn && topOn && _rb.velocity.y < 0)
        //{
        //    _detectingFallingEdge = true;
        //}
        //else
        //{
        //    _detectingFallingEdge = false;
        //}

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(topX, topY), 0.03f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector2(bottomX, bottomY), 0.03f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetector : MonoBehaviour
{
    public float topX, topY, bottomX, bottomY;
    public bool topOn, bottomOn;
    private bool _detectingFallingEdge;
    private bool _detectingRisingEdge;
    private StateMachineManager _machineManager;
    private Rigidbody2D _rb;
    [SerializeField] LayerMask _groundLayer;

    private void Awake()
    {
        _machineManager = GetComponent<StateMachineManager>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        topOn = Physics2D.OverlapCircle(new Vector2(transform.position.x + topX * _machineManager.direction,
                                                    transform.position.y + topY),
                                                    0.01f,
                                                    _groundLayer);

        bottomOn = Physics2D.OverlapCircle(new Vector2(transform.position.x + bottomX * _machineManager.direction,
                                                       transform.position.y + bottomY),
                                                       0.01f,
                                                       _groundLayer);

        if(bottomOn && !topOn && _rb.velocity.y < 0)
        {
            _detectingRisingEdge = true;
        }
        else
        {
            _detectingRisingEdge = false;
        }

        if (!bottomOn && topOn && _rb.velocity.y < 0)
        {
            _detectingFallingEdge = true;
        }
        else
        {
            _detectingFallingEdge = false;
        }
    }

}

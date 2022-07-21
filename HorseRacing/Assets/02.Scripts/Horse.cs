using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    private float _moveDistance;
    private float _targetDistance;
    private bool _doMove;

    public bool isFinish = false;

    public void StartMove(float target)
    {
        _doMove = true;
        _targetDistance = target;
    }

    private void FixedUpdate()
    {
        if(!isFinish && _doMove && _moveDistance < _targetDistance)
            Move();
    }

    void Move()
    {
        float range = Random.Range(minSpeed, maxSpeed);
        Vector3 moveVec = Vector3.forward * range * Time.fixedDeltaTime;
        transform.Translate(moveVec);

        _moveDistance += moveVec.z;

        if(_moveDistance >= _targetDistance)
            isFinish = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if (other.gameObject.name == "finish")
                isFinish = true;
        }
    }
}

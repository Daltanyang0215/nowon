using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Transform _tr;
    private Enemy _enemy;
    private Pathfinder _pathFinder;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    public float speed = 1.0f;
    private List<Transform> _wayPoints;
    private int _wayPointIndex = 0;
    private Transform _nextWayPoint;
    private float _originY;
    private float _offsetY;
    private Vector3 _targetPos;
    private Vector3 _dir;
    private float _posTolerance = 0.05f;

    public void SetStartEnd(Transform start, Transform end)
    {
        _start = start;
        _end = end;
    }
    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _enemy = GetComponent<Enemy>();
        _pathFinder = GetComponent<Pathfinder>();
    }

    private void Start()
    {
        _originY = _tr.position.y+ _offsetY;
        if (!_pathFinder.FindOptimizedPath(_start, _end, out _wayPoints))
        {
            throw new System.Exception("길찾기 실패");
        }
            _nextWayPoint = _wayPoints[0];
    }

    private void FixedUpdate()
    {
        _targetPos = new Vector3(_nextWayPoint.position.x, _originY, _nextWayPoint.position.z);
        _dir = (_targetPos - _tr.position).normalized;


        if (Vector3.Distance(_targetPos, _tr.position) < _posTolerance)
        {
            if (TryGetNextPoint(_wayPointIndex, out _nextWayPoint))
            {
                _wayPointIndex++;
            }
            else
            {
                OnReachedToEnd();
            }
        }

        _tr.LookAt(_targetPos);
        _tr.Translate(speed * Time.fixedDeltaTime * _dir, Space.World);
    }

    private void OnReachedToEnd()
    {
        Player.instance.life -= 1;
        _enemy.Die();
    }

    public bool TryGetNextPoint(int curretPosintIndex, out Transform nextPoint)
    {
        nextPoint = null;

        if (curretPosintIndex < _wayPoints.Count - 1)
        {
            nextPoint = _wayPoints[curretPosintIndex + 1];
        }

        return nextPoint;
    }
}

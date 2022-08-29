using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isDetected
    {
        get => _detectedGround != null ? true : false;
    }
    public bool isIgnoringGround { get; private set; }
    private CapsuleCollider2D _col;
    private Vector2 _size;
    private Vector2 _center;

    private Collider2D _detectedGround;
    private Collider2D _lastGround;
    private Collider2D _ignoringGround;

    [SerializeField] private LayerMask _targetLayer;

    public void IgnoreLastGround()
    {
        _ignoringGround = _lastGround;
        if (_ignoringGround != null)
        {
            StartCoroutine(E_IgnoreGroundUntilPassedIt(_ignoringGround));
        }
    }

    private IEnumerator E_IgnoreGroundUntilPassedIt(Collider2D targetCol)
    {
        isIgnoringGround = true;
        Physics2D.IgnoreCollision(_col, targetCol, true);
        float targetColCenter = targetCol.transform.position.y + targetCol.offset.y;
        yield return new WaitUntil(() => _col.transform.position.y < targetColCenter);

        yield return new WaitUntil(() =>
        {
            bool isPassed = false;
            if (targetCol != null)
            {
                targetColCenter = targetCol.transform.position.y + targetCol.offset.y;

                if (_col.transform.position.y > targetColCenter || _col.transform.position.y + _col.size.y/4 < targetColCenter)
                {
                    isPassed = true;
                }
            }
            else
            {
                isPassed = true;
            }
            return isPassed;
        });

        Physics2D.IgnoreCollision(_col,targetCol, false);
        isIgnoringGround = false;
    }

    private void Awake()
    {
        _col = transform.Find("Collision").GetComponent<CapsuleCollider2D>();
        _size.x = _col.size.x / 3;
        _size.y = 0.005f;
    }

    private void Update()
    {
        _center.x = transform.position.x - _col.offset.x;
        _center.y = transform.position.y + _col.offset.y - (_col.size.y + _size.y) / 2 - 0.01f;
        _detectedGround = Physics2D.OverlapBox(_center, _size, 0, _targetLayer);
        if (_detectedGround != null)
        {
            _lastGround = _detectedGround;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_center, _size);
    }
}

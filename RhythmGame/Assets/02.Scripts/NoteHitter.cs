using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum HitType
{
    Miss,
    Bad,
    Good,
    Great,
    Cool
}
public class NoteHitter : MonoBehaviour
{
    [SerializeField] private KeyCode _keyCode;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _pressedColor;
    private Color _originColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            ChangeColor();
            TryHitNote();
        }
        
        if (Input.GetKeyUp(_keyCode))
        {
            RollBackColor();
        }
    }

    private bool TryHitNote()
    {
         List<Collider2D> notes =  Physics2D.OverlapBoxAll(transform.position, new Vector3(transform.lossyScale.x / 2, Constants.HIT_JUDGE_RANGE_MISS), 0).ToList();

        if(notes.Count > 0)
        {
            notes.Sort((x, y) => x.transform.position.y.CompareTo(y.transform.position.y));
            float distance = Mathf.Abs(transform.position.y - notes[0].transform.position.y);

            HitType hitType = HitType.Miss;
            if (distance < Constants.HIT_JUDGE_RANGE_COOL)
            {
                hitType = HitType.Cool;
            }
            else if (distance < Constants.HIT_JUDGE_RANGE_GREAT)
            {
                hitType = HitType.Great;
            }
            else if (distance < Constants.HIT_JUDGE_RANGE_GOOD)
            {
                hitType = HitType.Good;
            }
            else if (distance < Constants.HIT_JUDGE_RANGE_BAD)
            {
                hitType = HitType.Bad;
            }

            notes[0].GetComponent<Note>().Hit(hitType);
            return true;
        }
        return false;
    }

    private void ChangeColor() => _spriteRenderer.color = _pressedColor;
    private void RollBackColor() => _spriteRenderer.color = _originColor;

    private void OnDrawGizmosSelected()
    {
        // 판정범위
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.lossyScale.x / 2, Constants.HIT_JUDGE_RANGE_MISS, 0));
    
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.lossyScale.x / 2, Constants.HIT_JUDGE_RANGE_BAD, 0));
    
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.lossyScale.x / 2, Constants.HIT_JUDGE_RANGE_GOOD, 0));
    
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.lossyScale.x / 2, Constants.HIT_JUDGE_RANGE_GREAT, 0));
    
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.lossyScale.x / 2, Constants.HIT_JUDGE_RANGE_COOL, 0));
    }
}

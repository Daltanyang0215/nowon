using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    private float _disappearTimer=0.5f;
    private float _disappearSpeed=2.0f;
    private float _moveSpeedY = 0.5f;
    private Color _color;

    public static DamagePopUp Create(Vector3 pos, int damage)
    {

    }

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        transform.position += _moveSpeedY * Time.deltaTime * Vector3.up ;

        if(_disappearTimer < 0)
        {
            _color.a -= _disappearSpeed * Time.deltaTime;
            _textMeshPro.color = _color;
            if(_color.a < 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            _disappearTimer -= Time.deltaTime;
        }
    }

    private void SetUp(int damage)
    {
        _textMeshPro.SetText(damage.ToString());
    }
}

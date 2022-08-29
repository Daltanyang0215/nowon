using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        FlowPlayer,
        Attack,
        Hurt,
        Die
    }

    [SerializeField] EnemyState enemyState;

    [SerializeField] private float _hp;

}

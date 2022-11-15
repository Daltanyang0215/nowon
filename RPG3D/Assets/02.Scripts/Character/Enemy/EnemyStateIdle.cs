using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : StateBase<EnemyStates>
{
    public EnemyStateIdle(EnemyStates stateType, Func<bool> condition, List<KeyValuePair<Func<bool>, EnemyStates>> transition, GameObject owner) : base(stateType, condition, transition, owner)
    {
    }
}

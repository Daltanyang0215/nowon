using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDie : StateBase<EnemyStates>
{
    public EnemyStateDie(EnemyStates stateType, Func<bool> condition, List<KeyValuePair<Func<bool>, EnemyStates>> transition, GameObject owner) : base(stateType, condition, transition, owner)
    {
    }
}

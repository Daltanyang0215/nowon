using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHurt : StateBase<EnemyStates>
{
    public EnemyStateHurt(EnemyStates stateType, Func<bool> condition, List<KeyValuePair<Func<bool>, EnemyStates>> transition, GameObject owner) : base(stateType, condition, transition, owner)
    {
    }

}

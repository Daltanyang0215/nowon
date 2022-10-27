using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHpPosionL : Item
{
    public override void OnEarn()
    {
        Player.instance.hp+=3;
    }
}

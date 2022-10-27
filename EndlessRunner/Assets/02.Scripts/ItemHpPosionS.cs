using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHpPosionS : Item
{
    public override void OnEarn()
    {
        Player.instance.hp++;
    }
}

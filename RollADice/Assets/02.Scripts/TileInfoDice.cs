using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfoDice : TileInfo
{
    public override void OnTile()
    {
        base.OnTile();
        GameManager.instance.diceNum++;
    }

}

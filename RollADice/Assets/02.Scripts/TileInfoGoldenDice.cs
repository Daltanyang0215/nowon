using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfoGoldenDice : TileInfoDice
{
    public override void OnTile()
    {
        base.OnTile();
        GameManager.instance.goldenDiceNum++;
    }
}

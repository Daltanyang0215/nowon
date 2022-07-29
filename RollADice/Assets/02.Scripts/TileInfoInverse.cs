using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfoInverse : TileInfo
{

    public override void OnTile()
    {
        base.OnTile();
        GameManager.instance.direction = Constants.DIRECTION_NEGATIVE;
    }

}

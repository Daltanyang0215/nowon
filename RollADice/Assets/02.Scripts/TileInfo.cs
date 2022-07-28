using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int index;
    public string tileName;
    public string discription;

    public virtual void OnTile()
    {
        Debug.Log($"{index+1} ¹øÂ° Ä­ÀÎ {tileName},{discription}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoStar : TileInfo
{
    private int _starValue;
    public int starValue
    {
        get
        {
            return _starValue;
        }
        set
        {
            _starValue = value;
            _starValueText.text = _starValue.ToString();
        }
    }

    [SerializeField] private Text _starValueText;

    private void Awake()
    {
        starValue = Constants.STAR_VALUE_INIT;
    }

    public override void OnTile()
    {
        base.OnTile();
        starValue++;

    }
}

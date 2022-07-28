using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private List<TileInfo> _tiles;
    private List<TileInfoStar> _starTiles = new List<TileInfoStar>();

    private int _starScore;
    private int starScroe
    {
        set
        {
            _starScore=value;
            _starScoreText.text = _starScore.ToString();
        }
    }
    [SerializeField] private Text _starScoreText;

    private int _diceNum;
    private int _goldenDiceNum;
    private int _tileCount;
    private int _current;

    private void Awake()
    {
        instance = this;
        _diceNum = Constants.DICE_NUM_INIT;
        _goldenDiceNum = Constants.GOLDEN_DICE_NUM_INIT;
        //_tiles.Sort((x,y) => x.index.CompareTo(y.index));
        _tiles.Sort((x,y) => x.index-y.index);
        _tileCount = _tiles.Count;

        foreach (var tile in _tiles)
        {
            // is -> 형 변환 가능 ? 가능시 true
            if(tile is TileInfoStar)
            {
                _starTiles.Add(tile as TileInfoStar);
            }
        }
    }

    public void RollADice()
    {
        if(_diceNum > 0)
        {
            int randomValue = Random.Range(1, 7);
            MovePlayer(randomValue);
        }
    }

    private void MovePlayer(int diceValue)
    {
        _current += diceValue;
        if(_current >= _tileCount)
            _current -= _tileCount;

        Player.Instance.MoveTo(_tiles[_current].transform);
        _tiles[_current].OnTile();
    }
}

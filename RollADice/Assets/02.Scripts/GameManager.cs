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
        get
        {
            return _starScore;
        }
        set
        {
            _starScore = value;
            _starScoreText.text = _starScore.ToString();
        }
    }
    [SerializeField] private Text _starScoreText;

    private int _diceNum;
    public int diceNum
    {
        get
        {
            return _diceNum;
        }
        set
        {
            _diceNum = value;
            _diceNumText.text = _diceNum.ToString();
        }
    }
    [SerializeField] private Text _diceNumText;

    private int _goldenDiceNum;
    public int goldenDiceNum
    {
        get
        {
            return _goldenDiceNum;
        }
        set
        {
            _goldenDiceNum = value;
            _goldenDiceNumText.text = _goldenDiceNum.ToString();
        }
    }
    [SerializeField] private Text _goldenDiceNumText;

    // direction 1: positive, -1 L negative
    private int _direction;
    public int direction
    {
        get
        {
            return _direction;
        }
        set
        {
            if (value < 0)
            {
                _direction = -1;
                _inverseIcon.SetActive(true);
            }
            else
            {
                _direction = 1;
                _inverseIcon.SetActive(false);
            }

        }
    }
    [SerializeField] private GameObject _inverseIcon;

    private int _tileCount;
    private int _current = -1;

    private void Awake()
    {
        instance = this;
        diceNum = Constants.DICE_NUM_INIT;
        goldenDiceNum = Constants.GOLDEN_DICE_NUM_INIT;
        direction = Constants.DIRECTION_POSITIVE;
        //_tiles.Sort((x,y) => x.index.CompareTo(y.index));
        _tiles.Sort((x, y) => x.index - y.index);
        _tileCount = _tiles.Count;
        foreach (var tile in _tiles)
        {
            // is -> 형 변환 가능 ? 가능시 true
            if (tile is TileInfoStar)
            {
                _starTiles.Add(tile as TileInfoStar);
            }
        }
    }
    private void Start()
    {
        DiceAnimationUI.instance.RegisterCallBack(MovePlayer);
    }

    public void RollADice()
    {
        if (diceNum > 0 && DiceAnimationUI.instance.isPlaying == false)
        {
            diceNum--;
            int randomValue = Random.Range(1, 7);
            DiceAnimationUI.instance.DoDiceAnimation(randomValue);
        }
    }

    public void RollAGoldenDice(int diceValue)
    {
        if (goldenDiceNum > 0 && DiceAnimationUI.instance.isPlaying == false)
        {
            goldenDiceNum--;
            DiceAnimationUI.instance.DoDiceAnimation(diceValue);
        }
    }


    private void MovePlayer(int diceValue)
    {
        if (direction > 0)
        {
            CalcPassedStarTiles(_current, diceValue); // 별 획득 메소드
            _current += diceValue;
            if (_current >= _tileCount)
                _current -= _tileCount;

        }
        else
        {
            _current -= diceValue;
            if (_current < 0)
                _current += _tileCount;
            direction = Constants.DIRECTION_POSITIVE;
        }
        Player.Instance.MoveTo(_tiles[_current].transform);
        _tiles[_current].OnTile();
    }

    private void CalcPassedStarTiles(int previous, int totalMove)
    {
        foreach (TileInfoStar starTile in _starTiles)
        {
            if (starTile.index > previous && starTile.index <= previous + totalMove)
            {
                starScroe += starTile.starValue;
            }
        }
    }

}

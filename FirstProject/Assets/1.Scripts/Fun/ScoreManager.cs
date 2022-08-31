using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int _score;
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            _scoreText.text = $"����Ʈ�� ���� �� : {_score}";
        }
    }
    [SerializeField]
    private Text _scoreText;


    private void Awake()
    {
        instance = this;
    }
}

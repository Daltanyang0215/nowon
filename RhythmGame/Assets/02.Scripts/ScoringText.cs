using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringText : MonoBehaviour
{
    public static ScoringText instance;

    private void Awake()
    {
        instance = this; 
    }

    private int _score;

    public int score
    {
        set
        {
            if (_isScoring)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(E_Scoring(_score, value));
            _score = value;
        }
        get
        {
            return _score;
        }
    }
    [SerializeField] private Text _text;
    [SerializeField] private float _scoringTime = 0.1f;
    private Coroutine _coroutine = null;
    private bool _isScoring = false;    

    IEnumerator E_Scoring(int before, int after)
    {
        int delta = (int)((after - before) / _scoringTime);
        _isScoring = true;
        while (before < after)
        {
            before += (int)(delta * Time.deltaTime);
            if (before > after)
                before = after;
            _text.text = before.ToString();

            yield return null;
        }
        _isScoring = false;
        _coroutine = null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceAnimationUI : MonoBehaviour
{
    public static DiceAnimationUI instance;
    private List<Sprite> sprites = new List<Sprite>();

    private float _timer;
    [SerializeField] private Image _Image;
    [SerializeField] private float _animationDelay;
    [SerializeField] private float _animationTime;

    public bool isPlaying { get; private set; }
    public delegate void AfterAnimation(int diceValue);
    event AfterAnimation OnAnimationFinish;

    public void RegisterCallBack(AfterAnimation afterAnimation)
    {
        OnAnimationFinish += afterAnimation;
    }

    private void Awake()
    {
        instance = this;
        LoadSprites();
    }

    private void LoadSprites()
    {
        Sprite[] tmpSprites = Resources.LoadAll<Sprite>("DiceImages");

        for (int i = 0; i < tmpSprites.Length; i++)
            sprites.Add(tmpSprites[i]);
    }

    private void Update()
    {
        //if (_timer < 0)
        //{
        //    _Image.sprite = sprites[Random.Range(0, sprites.Count)];
        //    _timer = _animationDelay;
        //}
        //else
        //{
        //    _timer -= Time.deltaTime;
        //}
    }

    public void DoDiceAnimation(int diceValue)
    {
        StartCoroutine(E_DiceAnimation(diceValue));
        
    }

    IEnumerator E_DiceAnimation(int diceValue)
    {
        isPlaying = true;
        float elapsedTime = 0;
        while (elapsedTime < _animationTime)
        {
            if (_timer < 0)
            {
                _Image.sprite = sprites[Random.Range(6, sprites.Count)];
                _timer = _animationDelay;
            }
            _timer -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        OnAnimationFinish(diceValue);
        _Image.sprite = sprites[diceValue-1];
        isPlaying = false;
    }
}

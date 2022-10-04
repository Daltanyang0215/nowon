using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class LevelCompletePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private Transform _star1;
    [SerializeField] private Transform _star2;
    [SerializeField] private Transform _star3;
    [SerializeField] private Button _backToLobbyButton;
    [SerializeField] private Button _replayButton;
    [SerializeField] private Button _nextButton;

    public void Setup(int level, float lifeRatio, UnityAction buttonAction)
    {
        _level.text = level.ToString();
        StartCoroutine(E_StartAnimation(lifeRatio));
        _backToLobbyButton.onClick.AddListener(buttonAction);
        _replayButton.onClick.AddListener(buttonAction);
        _nextButton.onClick.AddListener(buttonAction);
        gameObject.SetActive(true);
    }

    IEnumerator E_StartAnimation(float lifeRatio)
    {
        yield return new WaitForSeconds(0.3f);
        if (lifeRatio >= 1f / 3f)
            _star1.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (lifeRatio >= 2f / 3f)
            _star2.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (lifeRatio >= 5f / 6f)
            _star3.GetChild(0).gameObject.SetActive(true);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class LevelFailedPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private Button _backToLobbyButton;
    [SerializeField] private Button _replayButton;

    public void Setup(int level,UnityAction buttonAction)
    {
        _level.text = level.ToString();
        gameObject.SetActive(true);
        _backToLobbyButton.onClick.AddListener(buttonAction);
        _replayButton.onClick.AddListener(buttonAction);
    }
}

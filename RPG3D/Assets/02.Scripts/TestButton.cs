using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public static TestButton instance;
    public Button button;

    public void Subscribe(GameObject subscriber, UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    private void Awake()
    {
        instance = this;
        button.onClick.AddListener(() => Debug.Log("HI"));
    }
}

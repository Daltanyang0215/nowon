using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public static GamePlay instance;

    public enum States
    {
        Idle,
        PlayStartEvents,
        WaitForStartEvents,
        PlayStage,
        WaitForStateFinished,
        NextStage,
        LevelCompleted,
        LevelFailed,
        WaitForUser
    }
    public States state;

    public LevelInfo levelInfo;

    public void StartLevel()
    {
        if (state != States.Idle) return;
        state = States.PlayStartEvents;
    }

    IEnumerator E_Init()
    {
        yield return new WaitUntil(()=> Player.Instance);
        yield return new WaitUntil(()=> TowerPlayer.Instance);
        TowerPlayer.Instance.SetUP(levelInfo.lifeInit, levelInfo.moneyInit);

        StartLevel();
    }

    private void Awake()
    {
        instance = this;

        StartCoroutine(E_Init());
    }

    private void Update()
    {
        switch (state)
        {
            case States.Idle:
                break;
            case States.PlayStartEvents:
                break;
            case States.WaitForStartEvents:
                break;
            case States.PlayStage:
                break;
            case States.WaitForStateFinished:
                break;
            case States.NextStage:
                break;
            case States.LevelCompleted:
                break;
            case States.LevelFailed:
                break;
            case States.WaitForUser:
                break;
            default:
                break;
        }
    }
}

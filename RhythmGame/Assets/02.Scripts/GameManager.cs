using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유한상태머신 (FSM finite State Machine) 절차 반복

public class GameManager : MonoBehaviour
{
    public enum State
    {
        Idle,
        WaitForSongSelected,
        StartGame,
        WaitForGameFinished,
        ShowScore
    }
    private State _state;

    private void Update()
    {
      
    }

    private void WorkFlow()
    {
        switch (_state)
        {
            case State.Idle:
                break;
            case State.WaitForSongSelected:
                break;
            case State.StartGame:
                break;
            case State.WaitForGameFinished:
                break;
            case State.ShowScore:
                break;
            default:
                break;
        }
    }
}

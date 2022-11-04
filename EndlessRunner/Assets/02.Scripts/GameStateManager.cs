using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum GameStates
{
    None,
    Idle,
    Play,
    Paused
}

public class GameStateManager
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new GameStateManager();
            return _instance;
        }
    }

    public GameStates Currnt { get; private set;    }
    public event Action<GameStates> OnStateChanged;

    public void SetState(GameStates newState)
    {
        if (newState == Currnt) return;

        Currnt = newState;
        OnStateChanged?.Invoke(Currnt);
    }
}

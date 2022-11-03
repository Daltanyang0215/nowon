using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public void StartGmae()
    {
        Player.instance.StartMove();
        GameStateManager.Instance.SetState(GameStates.Play);
    }

    public void PausedGame()
    {
        GameStateManager.Instance.SetState(GameStates.Paused);
    }

    private void Start()
    {
        GameStateManager.Instance.SetState(GameStates.Paused);   
    }


}

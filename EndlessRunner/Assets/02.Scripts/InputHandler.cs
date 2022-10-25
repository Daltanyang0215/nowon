using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public const KeyCode SHORTCUT_PLAYER_JUMP = KeyCode.UpArrow;
    public const KeyCode SHORTCUT_PLAYER_MOVE_LEFT = KeyCode.LeftArrow;
    public const KeyCode SHORTCUT_PLAYER_MOVE_RIGHT = KeyCode.RightArrow;
    public const KeyCode SHORTCUT_PLAYER_SLIDE = KeyCode.DownArrow;

    private static Dictionary<KeyCode, Action> keyDownActions = new Dictionary<KeyCode, Action>();

    public static void RegisterKeyDownAction(KeyCode key, Action keyDownaction)
    {
        if (keyDownActions.ContainsKey(key))
        {
            keyDownActions[key] += keyDownaction;
        }
        else
        {
            keyDownActions.Add(key, keyDownaction);
        }
    }

    public static void ClearKeyDownAction (KeyCode key) => keyDownActions.Remove(key);

    private void Update()
    {
        foreach (var pair in keyDownActions)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                pair.Value?.Invoke();
            }
        }
    }
}

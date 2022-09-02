using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMoveButton : MonoBehaviour
{
    public void onClick(string SceneName)
    {
        SceneMover.MoveTo(SceneName);
    }

}

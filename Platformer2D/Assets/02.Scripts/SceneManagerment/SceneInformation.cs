using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInformation : MonoBehaviour
{
    static public SceneInformation instance;
    static public bool isSceneLoaded;
    static public string oldSceneName;
    static public string newSceneName;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    private void Start()
    {
        SceneManager.sceneUnloaded += delegate
        {
            isSceneLoaded = false;
        };

        SceneManager.sceneLoaded += delegate
        {
            oldSceneName = newSceneName;
            newSceneName= SceneManager.GetActiveScene().name;
            isSceneLoaded = true;
        };
    }
}

using UnityEngine.SceneManagement;

public class SceneMover
{
    public static void MoveTo(string sceneName) => SceneManager.LoadScene(sceneName);
    public static void MoveTo(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
}

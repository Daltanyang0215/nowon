using UnityEngine;

[System.Serializable]
public class NoteData
{
    public KeyCode ketCode;
    public float time;
    public float speedScale;

    public NoteData(KeyCode keyCode, float time, float speedScale)
    {
        this.ketCode = keyCode;
        this.time = time;
        this.speedScale = speedScale;
    }
}

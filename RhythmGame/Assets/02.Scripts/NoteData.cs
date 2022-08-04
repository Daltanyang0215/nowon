using UnityEngine;

[System.Serializable]
public class NoteData
{
    public KeyCode keyCode;
    public float time;
    public float speedScale;

    public NoteData(KeyCode keyCode, float time, float speedScale)
    {
        this.keyCode = keyCode;
        this.time = time;
        this.speedScale = speedScale;
    }
}

using UnityEngine;
using UnityEngine.Video;
using UnityEditor;
public class NotesMaker : MonoBehaviour
{
    private SongData _songData;
    private KeyCode[] _keyCodes =
    {
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.Space,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L
    };

    [SerializeField] private VideoPlayer _videoPlayer;
    public bool isRecording
    {
        get => _videoPlayer.isPlaying;
    }

    private void Update()
    {
        if (isRecording)
        {
            Recording();
        }
    }

    public void ONRecordButtonClick()
    {
        _songData = new SongData();
        _songData.videoName = _videoPlayer.clip.name;
        _videoPlayer.Play();
    }
    public void OnStopRecordButtonClick()
    {
        SaveSongData();
        _videoPlayer.Stop();
    }

    private void Recording()
    {
        foreach (KeyCode keyCode in _keyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                CreateNoteData(keyCode);
            }
        }
    }

    private void CreateNoteData(KeyCode keyCode)
    {
        float roundedtime = (float)System.Math.Round(_videoPlayer.time, 2);
        NoteData noteData = new NoteData(keyCode, roundedtime, 1f);
        Debug.Log($"노트 데이터 생성 : {keyCode}, {roundedtime}");
        _songData.notes.Add(noteData);
    }

    private void SaveSongData()
    {
        string dir = EditorUtility.SaveFilePanel("저장할 폴더를 지정해주세요",
                                                 "",
                                                 _songData.videoName,
                                                 "json");
        System.IO.File.WriteAllText(dir, JsonUtility.ToJson(_songData));
    }
}

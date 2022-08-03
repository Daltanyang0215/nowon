using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SongSelector : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    public static SongSelector instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public VideoClip clip { get; private set; }
    public SongData songData { get; private set; }
    public bool isDataLoaded { get; private set; }
    public void LoadSongData(string clipName)
    {

        try
        {
            clip = Resources.Load<VideoClip>($"VideoClips/{clipName}");
            TextAsset songDataText = Resources.Load<TextAsset>($"SongsData/{clip.name}");
            songData = JsonUtility.FromJson<SongData>(songDataText.ToString());
            isDataLoaded = true;
            Debug.Log($"Songdata Loaded : {songData.videoName}");
        }
        catch
        {
            Debug.Log($"Songdata Loaded Fail . {clipName}");
            isDataLoaded = false;
        }
    }
}

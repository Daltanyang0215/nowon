using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    public string videoName;
    public List<NoteData> notes;

    public SongData()
    {
        notes = new List<NoteData>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public KeyCode keyCode;
    [SerializeField]
    private GameObject _notePrefab;

    public Note SpawnNote()
    {
        GameObject note = Instantiate(_notePrefab,transform.position,Quaternion.identity);
        note.transform.localScale = transform.lossyScale;
        return note.GetComponent<Note>();
    }

    public Note SpawnNote_Debug(float _time)
    {
        GameObject note = Instantiate(_notePrefab, transform.position + Vector3.up * _time, Quaternion.identity);
        note.transform.localScale = transform.lossyScale;
        return note.GetComponent<Note>();
    }
}

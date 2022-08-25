using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLock : MonoBehaviour
{
    [SerializeField]
    private Renderer _mesh;

    private bool _islock;
    public bool islock { get { return _islock; } }
    private void Awake()
    {
        if (_mesh == null)
            _mesh = GetComponent<Renderer>();
    }

    public void Lock(bool _lock)
    {
        if (_lock)
        {
            _islock = true;
            _mesh.materials[0].SetFloat("_isLock",1);
        }
        else
        {
            _islock = false;
            _mesh.materials[0].SetFloat("_isLock", 0);
        }
    }

}

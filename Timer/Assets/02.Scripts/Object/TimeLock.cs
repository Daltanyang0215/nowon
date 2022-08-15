using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLock : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _mesh;

    private bool _islock;
    public bool islock { get { return _islock; } }
    private void Awake()
    {
        if (_mesh == null)
            _mesh = GetComponent<MeshRenderer>();
    }

    public void Lock(bool _lock)
    {
        if (_lock)
        {
            _islock = true;
            _mesh.materials[0].color = Color.black;
        }
        else
        {
            _islock = false;
            _mesh.materials[0].color = Color.white;
        }
    }

}

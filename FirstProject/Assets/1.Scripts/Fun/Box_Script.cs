using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Box_Script : MonoBehaviour
{
    private Transform _lockOnAnchor;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _lockOnAnchor = transform.GetChild(0);
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(_lockOnAnchor.gameObject.activeSelf)
            _lockOnAnchor.LookAt(Camera.main.transform);
    }

    private void OnEnable()
    {
        _lockOnAnchor.gameObject.SetActive(false);
        _meshRenderer.material.color = Color.white;
        Targeting(false);
    }

    public void Targeting(bool targeting)
    {
        _meshRenderer.material.color = targeting ? Color.blue : Color.white;
        _lockOnAnchor.gameObject.SetActive(targeting);
    }
}

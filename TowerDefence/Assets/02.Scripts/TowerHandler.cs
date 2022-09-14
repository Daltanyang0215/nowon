using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerHandler : MonoBehaviour
{
    public static TowerHandler instance;
    public bool isSelected => _selectedTowerInfo;

    private GameObject _ghostTwoer;
    private TowerInfo _selectedTowerInfo;
    private Camera _camera;
    private Ray _ray;
    private RaycastHit _hit;
    [SerializeField] private LayerMask _nodeLayer;

    public void SetTower(TowerInfo towerInfo)
    {
        _selectedTowerInfo = towerInfo;
        gameObject.SetActive(true);
        if (_ghostTwoer != null)
        {
            Destroy(_ghostTwoer);
        }
        if(TowerAssets.instance.TryGetTower(_selectedTowerInfo.name,out GameObject ghostTowerPrefab))
        {
            _ghostTwoer = Instantiate(ghostTowerPrefab);
        }
        else
        {
            throw new System.Exception("고스트타워 참조 실패");
        }
    }

    public void Clear()
    {
        _ghostTwoer = null;
        _selectedTowerInfo = null;
        gameObject.SetActive(false);
    }

    public void SetGhostTowerPosition(Vector3 targetPos)
    {
        _ghostTwoer.transform.position = targetPos;
    }

    private void Awake()
    {
        if(instance != null)
            Destroy(instance);
        instance = this;

        _camera = Camera.main;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_ghostTwoer == null) return;
        _ray =  _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.Log(Mouse.current.position.ReadValue());
        if (Physics.Raycast(_ray, out _hit, float.PositiveInfinity, _nodeLayer))
        {
            SetGhostTowerPosition(_hit.collider.transform.position);
            _ghostTwoer.SetActive(true);
        }
        else
        {
            _ghostTwoer.SetActive(false);
        }

    }
}

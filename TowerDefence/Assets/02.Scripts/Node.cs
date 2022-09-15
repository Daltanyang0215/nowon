using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool IsTowerExist => _towerBuilt;
    private Tower _towerBuilt;
    private Renderer _renderer;

    private Color _originalColor;
    [SerializeField] private Color _buildAvailableColor;
    [SerializeField] private Color _buildNotAvailableColor;

    public bool TryBuildTowerHere(string towerName)
    {
        bool isOK = false;
        if (IsTowerExist)
        {
            //Destroy(_towerBuilt);
            Debug.Log("해당 위치에 이미 걸설된 타워가 있습니다.");
            return false;
        }

        if(TowerAssets.instance.TryGetTower(towerName,out GameObject tower))
        {
            _towerBuilt = Instantiate(tower,
                        transform.position + Vector3.up * 0.5f,
                        Quaternion.identity,
                        transform).GetComponent<Tower>();
            isOK = true;
        }
        return isOK;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }


    private void OnMouseEnter()
    {
        if (!IsTowerExist)
        {
            _renderer.material.color = _buildAvailableColor;
        }
        else
        {
            _renderer.material.color = _buildNotAvailableColor;
        }
        NodeManager.mouseOnNode = this;
    }
    private void OnMouseExit()
    {
        _renderer.material.color = _originalColor;
    }

}

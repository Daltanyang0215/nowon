using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAssets : MonoBehaviour
{
    private static TowerAssets _instance;
    public static TowerAssets instance
    {
        get
        {
            if (_instance == null)
                _instance = Instantiate( Resources.Load<TowerAssets>("Assets/TowerAssets"));
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] private List<GameObject> _towers;
    [SerializeField] private List<GameObject> _ghostTowers;

    public bool TryGetTower(string towerName , out GameObject tower)
    {
        tower = _towers.Find(x => x.name == towerName);
        return tower;
        // 유니티 엔진 중 게임오브젝트 리턴시 null이면 bool false 반환
    }
    public bool TryGetGhostTower(string towerName , out GameObject ghostTower)
    {
        ghostTower = _ghostTowers.Find(x => x.name == towerName+"Ghost");
        return ghostTower;
    }

}

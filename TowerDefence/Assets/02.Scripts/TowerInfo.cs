using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerTypes
{
    MachineGun,
    RocketLauncher,
    Laser
}

[CreateAssetMenu(menuName ="TowerDefence/CreateTowerInfo", fileName = "TowerInfo")]
public class TowerInfo : ScriptableObject
{
    public TowerTypes Type;
    public int upgradeLevel;
    public int buildPrice;
    public int sellPrice;
    public Sprite icon;
}

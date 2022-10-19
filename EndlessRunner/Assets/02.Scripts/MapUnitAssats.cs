using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnitAssats : MonoBehaviour
{
    private static MapUnitAssats _instance;
    public static MapUnitAssats Instance
    {
        get
        {
            if (_instance == null)
                _instance = Instantiate(Resources.Load<MapUnitAssats>("MapUnitAssets"));
            return _instance;
        }
    }

    [SerializeField] private List<MapUnits> _mapUnits;

    public MapUnits GetRandomMapUnit()
    {
        return _mapUnits[Random.Range(0, _mapUnits.Count)];
    }
}

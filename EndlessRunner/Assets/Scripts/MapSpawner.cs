using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pos
{
    Left,
    Center,
    Right
}

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private Transform _mapUnitsLeftParent;
    [SerializeField] private Transform _mapUnitsCenterParent;
    [SerializeField] private Transform _mapUnitsRightParent;

    private LinkedList<MapUnits> _mapUnitsLeft = new LinkedList<MapUnits>();
    private LinkedList<MapUnits> _mapUnitsCenter = new LinkedList<MapUnits>();
    private LinkedList<MapUnits> _mapUnitsRight = new LinkedList<MapUnits>();

    private MapUnits _tmpMapUnit;

    private void Awake()
    {
        foreach (MapUnits mapUnits in _mapUnitsLeftParent.GetComponentsInChildren<MapUnits>())
        {
            _mapUnitsLeft.AddLast(mapUnits);
            mapUnits.OnReachedToEnd += () =>
            {
                _mapUnitsLeft.Remove(mapUnits);
                Spawn(Pos.Left);
            };

        }

        foreach (MapUnits mapUnits in _mapUnitsCenterParent.GetComponentsInChildren<MapUnits>())
        {
            _mapUnitsCenter.AddLast(mapUnits);
            mapUnits.OnReachedToEnd += () =>
            {
                _mapUnitsCenter.Remove(mapUnits);
                Spawn(Pos.Center);
            };
        }

        foreach (MapUnits mapUnits in _mapUnitsRightParent.GetComponentsInChildren<MapUnits>())
        {
            _mapUnitsRight.AddLast(mapUnits);
            mapUnits.OnReachedToEnd += () =>
            {
                _mapUnitsRight.Remove(mapUnits);
                Spawn(Pos.Right);
            };
        }


    }

    private void Spawn(Pos pos)
    {
        _tmpMapUnit = MapUnitAssats.Instance.GetRandomMapUnit();

        _tmpMapUnit.OnReachedToEnd += () =>
        {
            _mapUnitsLeft.Remove(_tmpMapUnit);
            Spawn(pos);
        };

        MapUnits mapUnit;
        MapUnits tmpmapUnit;

        switch (pos)
        {
            case Pos.Left:
                mapUnit = _mapUnitsLeft.Last.Value;
                tmpmapUnit = Instantiate(_tmpMapUnit,
                                                  mapUnit.transform.position + Vector3.forward * (mapUnit.length * 0.5f + _tmpMapUnit.length * 0.5f),
                                                  Quaternion.identity,
                                                  _mapUnitsLeftParent);
                _mapUnitsLeft.AddLast(tmpmapUnit);
                tmpmapUnit.OnReachedToEnd += () =>
                {
                    _mapUnitsLeft.Remove(_tmpMapUnit);
                    Spawn(pos);
                };
                break;
            case Pos.Center:
                mapUnit = _mapUnitsCenter.Last.Value;
                tmpmapUnit = Instantiate(_tmpMapUnit,
                                                 mapUnit.transform.position + Vector3.forward * (mapUnit.length * 0.5f + _tmpMapUnit.length * 0.5f),
                                                 Quaternion.identity,
                                                 _mapUnitsCenterParent);
                _mapUnitsCenter.AddLast(tmpmapUnit);
                tmpmapUnit.OnReachedToEnd += () =>
                {
                    _mapUnitsLeft.Remove(_tmpMapUnit);
                    Spawn(pos);
                };
                break;
            case Pos.Right:
                mapUnit = _mapUnitsRight.Last.Value;
                tmpmapUnit = Instantiate(_tmpMapUnit,
                                                 mapUnit.transform.position + Vector3.forward * (mapUnit.length * 0.5f + _tmpMapUnit.length * 0.5f),
                                                 Quaternion.identity,
                                                 _mapUnitsRightParent);
                _mapUnitsRight.AddLast(tmpmapUnit);
                tmpmapUnit.OnReachedToEnd += () =>
                {
                    _mapUnitsLeft.Remove(_tmpMapUnit);
                    Spawn(pos);
                };
                break;
            default:
                break;
        }
    }
}

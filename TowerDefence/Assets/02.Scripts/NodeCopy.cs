using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCopy : MonoBehaviour
{
    [SerializeField] private GameObject _node;
    [SerializeField] private Transform _nodes;

    [SerializeField] private int _minX;
    [SerializeField] private int _maxX;
    [SerializeField] private int _minZ;
    [SerializeField] private int _maxZ;

    private void Start()
    {
        for (int j = _minZ; j < _maxZ; j++)
        {
            for (int i = _minX; i < _maxX; i++)
            {
                bool isok = true;
                for (int k = 0; k < _nodes.childCount; k++)
                {
                    if(_nodes.GetChild(k).transform.position == new Vector3(i, 0, j))
                    {
                        isok = false;
                    }
                }
                if(isok)
                Instantiate(_node, new Vector3(i, 0, j), Quaternion.identity).transform.parent = _nodes;
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private static Node _mouseOnNode;
    public static Node mouseOnNode
    {
        get { return _mouseOnNode; 
        }
        set
        {
            //if (TowerHandler.instance.isSelected)
            //{
            //    TowerHandler.instance.SetGhostTowerPosition(_mouseOnNode.transform.position);
            //}
                
        }
    }


}

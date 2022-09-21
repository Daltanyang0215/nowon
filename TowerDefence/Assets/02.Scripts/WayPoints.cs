using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static WayPoints instance;

    public Transform[] points;

    public bool TryGetNextPoint(int curretPosintIndex , out Transform nextPoint)
    {
        nextPoint = null;

        if(curretPosintIndex < points.Length - 1)
        {
            nextPoint = points[curretPosintIndex + 1];
        }

        return nextPoint;
    }

    private void Awake()
    {
        if(instance != null)
            Destroy(instance);
        instance = this;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public bool isDetected;

    [SerializeField] private LayerMask _wallLayer;

    private void OnTriggerStay(Collider other)
    {
        isDetected = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isDetected = false;
    }
}

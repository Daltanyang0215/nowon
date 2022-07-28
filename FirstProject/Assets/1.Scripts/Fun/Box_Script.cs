using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Box_Script : MonoBehaviour
{
    public PlayerBulletShot player;

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            player.TargetQueue(gameObject);
        }
    }

    private void OnMouseDown()
    {
        player.TargetQueue(gameObject);
    }

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void Targeting(bool targeting)
    {
        GetComponent<MeshRenderer>().material.color = targeting ? Color.red : Color.white;
    }

}

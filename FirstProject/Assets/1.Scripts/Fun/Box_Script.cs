using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Box_Script : MonoBehaviour
{
    public Player player;

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            player.blocks.Add(gameObject);
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void OnMouseDown()
    {
        player.blocks.Add(gameObject);
        GetComponent<MeshRenderer>().material.color = Color.red;

    }

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

}

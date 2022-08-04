using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Box_Script : MonoBehaviour
{
    private Transform lockOnAnchor;

    private void Awake()
    {
        lockOnAnchor = transform.GetChild(0);
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            PlayerBulletShot.Instance.TargetQueue(gameObject);
        }
    }

    private void OnMouseDown()
    {
        PlayerBulletShot.Instance.TargetQueue(gameObject);
    }

    private void Update()
    {
        if(lockOnAnchor.gameObject.activeSelf)
            lockOnAnchor.LookAt(Camera.main.transform);
    }


    private void OnDisable()
    {
        lockOnAnchor.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().material.color = Color.white;
        Targeting(false);
    }

    public void Targeting(bool targeting)
    {
        GetComponent<MeshRenderer>().material.color = targeting ? Color.blue : Color.white;
        lockOnAnchor.gameObject.SetActive(targeting);
    }

}

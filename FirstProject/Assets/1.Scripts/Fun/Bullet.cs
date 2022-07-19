using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;

    public float speed;

    private void Start()
    {
        transform.LookAt(target);
    }

    public void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,target.position , speed);   
    }

    public void Update()
    {
        if(!target.gameObject.activeSelf)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject == target.gameObject)
            {
                other.gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

}

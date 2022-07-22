using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _damage = 2f;

    private void Start()
    {
        Destroy(gameObject,3f);
        
    }


    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other!= null)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.GetComponent<Enemy>().hp -= _damage;
            }

        }
    }


}

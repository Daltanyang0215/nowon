using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform[] firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Shooting();
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, firePoint[0].position, firePoint[0].rotation);
            //Instantiate(bullet, firePoint[1].position, firePoint[1].rotation);
            //Instantiate(bullet, firePoint[2].position, firePoint[2].rotation);
        }

    }
}

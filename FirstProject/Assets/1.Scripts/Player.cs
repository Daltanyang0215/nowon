using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private float forwad, rotate;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
    }

    public void GetInput()
    {
        rotate = Input.GetAxis("Horizontal");
        forwad = Input.GetAxis("Vertical");
    }

    public void Move()
    {

        transform.Translate(Vector3.forward * forwad * speed * Time.deltaTime);
        transform.Rotate(new Vector3(0,rotate,0));
        
    }


}

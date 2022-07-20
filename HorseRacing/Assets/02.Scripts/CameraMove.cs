using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public List<Horse> horses = new List<Horse>(); 
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < horses.Count; i++)
        {
            if(transform.position.z < horses[i].transform.position.z)
            {
                transform.position = new Vector3(9, 7.5f, horses[i].transform.position.z);
            }
        }
        

    }
}

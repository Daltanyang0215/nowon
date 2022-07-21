using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera playerFollowingCam;


    //public List<Horse> horses = new List<Horse>();
    // Update is called once per frame

    private void Awake()
    {
        mainCam.enabled = true;
        playerFollowingCam.enabled = false;
    }


    void Update()
    {
        //for (int i = 0; i < horses.Count; i++)
        //{
        //    if (transform.position.z < horses[i].transform.position.z)
        //    {
        //        transform.position = new Vector3(9, 7.5f, horses[i].transform.position.z);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCam();
        }


    }
    void SwitchCam()
    {
        mainCam.enabled = !mainCam.enabled;
        playerFollowingCam.enabled = !playerFollowingCam.enabled;
    }
}

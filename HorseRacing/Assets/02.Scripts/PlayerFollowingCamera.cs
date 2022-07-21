using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowingCamera : MonoBehaviour
{
    public Vector3 cameraOffset;

    public List<Transform> targets = new List<Transform>();  

    private int targetIndex;
    // Start is called before the first frame update
    void Start()
    {
        targets = GameManager.instance.GetHorseTransforms();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            targetIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            targetIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            targetIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            targetIndex = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            targetIndex = 4;

        FollowTarget();
    }


    void FollowTarget()
    {
        transform.position = targets[targetIndex].position + cameraOffset;

    }

}

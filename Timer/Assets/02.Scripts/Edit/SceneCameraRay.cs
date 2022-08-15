using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SceneCameraRay : MonoBehaviour
{
    private void Update()
    {
//        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(
                Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                print("raycast hit!");
                Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, 5f);
                Debug.Log(hit.point);
            }
        }

    }


}

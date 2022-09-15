using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [Space]
    [Header("Camera")]
    private float rotateX, rotateY;
    [SerializeField] private float rotateX_Min, rotateX_Max;
    [SerializeField] private float rotateYSpeed;
    [SerializeField] private float rotateXSpeed;
    [SerializeField] private float cameraPosZ_Min;
    [SerializeField] private float cameraPosZ_Max;

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _towerUI;
    [SerializeField] private Transform _playerUI;
    private bool _isPlayerCamera;

    [SerializeField]
    private Transform cameraAnchor;
    private bool clampCTR = false;

    [SerializeField] private bool _rotateToPlayer;



    private void Update()
    {

        CameraRotate();
        //CameraZoom();
    }

    private void OnLook(InputValue inputValue)
    {
        Vector2 input = inputValue.Get<Vector2>();
        rotateY = input.x;
        rotateX = input.y;
        //clampCTR = Input.GetKey(KeyCode.LeftControl);
    }

    private void OnCameraSwap()
    {
        _isPlayerCamera = !_isPlayerCamera;

        if (_isPlayerCamera) // 플레이어 시점
        {
            _mainCamera.gameObject.SetActive(false);
            _playerCamera.gameObject.SetActive(true);
            _towerUI.gameObject.SetActive(false);
            _playerUI.gameObject.SetActive(true);
            Cursor.visible = false;
        }
        else // 타워 시점
        {
            _mainCamera.gameObject.SetActive(true);
            _playerCamera.gameObject.SetActive(false);
            _towerUI.gameObject.SetActive(true);
            _playerUI.gameObject.SetActive(false);
            Cursor.visible = true;
        }



    }

    void CameraRotate()
    {
        if (_mainCamera.gameObject.activeSelf) return;

        float tmp_x = cameraAnchor.eulerAngles.x - rotateX * rotateXSpeed;
        float tmp_y = transform.eulerAngles.y + rotateY * rotateYSpeed;

        tmp_x = tmp_x > 180 ? tmp_x - 360 : tmp_x; // 오일러 0~360 -> -180~180 으로 전환
        tmp_y = tmp_y > 180 ? tmp_y - 360 : tmp_y; // 오일러 0~360 -> -180~180 으로 전환

        if (rotateX_Max < tmp_x)
        {
            tmp_x = rotateX_Max;
        }
        else if (tmp_x < rotateX_Min)
        {
            tmp_x = rotateX_Min;
        }

        if (_rotateToPlayer)
        {
            transform.rotation = Quaternion.Euler(tmp_x, tmp_y, 0);
        }
        else
        {
            cameraAnchor.rotation = Quaternion.Euler(tmp_x, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Euler(0, tmp_y, 0);
        }

        //cameraAnchor.Rotate(Vector3.right, tmp_x);
        //cameraAnchor.Rotate(Vector3.left * rotateX * rotateXSpeed);
    }

    //void CameraZoom()
    //{
    //    if (Input.mouseScrollDelta.y > 0)
    //    {
    //        if (cameraAnchor.GetChild(0).localPosition.z + 0.5f > cameraPosZ_Min)
    //        {
    //            cameraAnchor.GetChild(0).localPosition = Vector3.forward * cameraPosZ_Min;
    //        }
    //        else cameraAnchor.GetChild(0).localPosition += Vector3.forward * 0.5f;
    //    }
    //    else if (Input.mouseScrollDelta.y < 0)
    //    {
    //        if (cameraAnchor.GetChild(0).localPosition.z - 0.5f < cameraPosZ_Max)
    //        {
    //            cameraAnchor.GetChild(0).localPosition = Vector3.forward * cameraPosZ_Max;
    //        }
    //        else cameraAnchor.GetChild(0).localPosition += Vector3.back * 0.5f;
    //    }
    //}

}

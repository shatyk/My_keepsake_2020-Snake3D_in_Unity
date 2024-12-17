using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDRotator : MonoBehaviour
{
    [SerializeField] private CameraRotateContr cameraRotateContr;

    private void Awake()
    {
        cameraRotateContr.ChangeCameraDirection += OnChangedCameraDirection;
    }
    private void OnChangedCameraDirection(DirectionsEnum newDir)
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y = 90 * (int)newDir;
        transform.rotation = Quaternion.Euler(rot);
    }
}

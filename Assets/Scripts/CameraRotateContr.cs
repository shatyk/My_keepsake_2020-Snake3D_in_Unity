using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateContr : MonoBehaviour
{
    public DirectionsEnum CameraDirection { get; private set; }

    public delegate void ChangeCameraDirectionHandl(DirectionsEnum cameraDir);
    public event ChangeCameraDirectionHandl ChangeCameraDirection;

    private void Awake()
    {
        CameraDirection = DirectionsEnum.Forward;
    }

    // Update is called once per frame
    void Update()
    {
        double rotate = transform.rotation.eulerAngles.y;
        /* юнити какаха не поддерживает новодения шарпов
        DirectionsEnum newCameraDirection = rotate switch
        {
            >= -45 and < 45 => (DirectionsEnum)0,
            >= 45 and < 135 => (DirectionsEnum)1,
            >= 135 and < 225 => (DirectionsEnum)2,
            >= 225 or < -45 => (DirectionsEnum)3,
            _ => throw new Exception("pizdos")
        };
        */

        DirectionsEnum newCameraDirection;
        if (rotate >= 315 || rotate < 45) newCameraDirection = (DirectionsEnum)0;
        else if (rotate >= 45 && rotate < 135) newCameraDirection = (DirectionsEnum)1;
        else if (rotate >= 135 && rotate < 225) newCameraDirection = (DirectionsEnum)2;
        else if (rotate >= 225 && rotate < 315) newCameraDirection = (DirectionsEnum)3;
        else throw new Exception("pizdos");

        if (newCameraDirection != CameraDirection)
        {
            CameraDirection = newCameraDirection;
            ChangeCameraDirection?.Invoke(CameraDirection);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum DirectionsEnum
{
    Top = -2, Down, Forward, Right, Backward, Left
}

public static class RotatorProc
{
    public static DirectionsEnum GetTurn(Vector3 oldCoord, Vector3 newCoord, DirectionsEnum oldDir)
    {
        if (newCoord.x > oldCoord.x && oldDir != DirectionsEnum.Backward) return DirectionsEnum.Forward;
        if (newCoord.x < oldCoord.x && oldDir != DirectionsEnum.Forward) return DirectionsEnum.Backward;
        if (newCoord.z > oldCoord.z && oldDir != DirectionsEnum.Right) return DirectionsEnum.Left;
        if (newCoord.z < oldCoord.z && oldDir != DirectionsEnum.Left) return DirectionsEnum.Right;
        if (newCoord.y > oldCoord.y && oldDir != DirectionsEnum.Down) return DirectionsEnum.Top;
        if (newCoord.y < oldCoord.y && oldDir != DirectionsEnum.Top) return DirectionsEnum.Down;

        return DirectionsEnum.Forward;
    } 
}

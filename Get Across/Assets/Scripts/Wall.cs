using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private float posX;
    private float posZ;
    private float posY;

    public bool isOnBoard = false;

    public void SetPosition(float x, float z)
    {
        posX = x;
        posZ = z;
        posY = 0.5f;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(posX, posY, posZ);
    }
    public bool IsOpenSpace(Vector3 snapPoint)
    {
        return snapPoint.x == posX && snapPoint.z == posZ;
    }
}

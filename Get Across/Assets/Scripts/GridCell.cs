using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{

    private float posX;
    private float posZ;
    private float posY;

    public GameObject objectOnGridCell = null;
    public bool isOccupied = false;
    
    public void SetPosition(float x, float z)
    {
        posX = x;
        posZ = z;
        posY = 0;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(posX,posY, posZ);
    }
}

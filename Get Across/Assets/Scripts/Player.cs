using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject player;
    private GameObject[,] grid;

    [SerializeField] public GameObject gameGrid;
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        grid = new GameObject[9, 9];
    }
    public void SpawnPlayer(int x, int z, float GridSpaceSize)
    {
        player = Instantiate(playerPrefab, new Vector3(x * GridSpaceSize, 1, z * GridSpaceSize), Quaternion.identity);
        player.transform.parent = transform;
        player.transform.name = "Player model";
    }

    public void MovePlayer(Vector3 moveTo)
    {
        grid = gameGrid.GetComponent<GameGrid>().gameGrid;
        for (int z = 0; z < 9; z++)
        {
            for (int x = 0; x < 9; x++)
            {
                GridCell gridCell = grid[x, z].GetComponentInChildren<GridCell>();
                if (gridCell.isOccupied)
                {
                    if (CellIsNear(x,z,grid, moveTo))
                    {
                        moveTo.y = 1;
                        player.transform.position = moveTo;
                        gridCell.isOccupied = false;

                    }
                }
            }
        }

    }
    private bool CellIsNear(int x, int z, GameObject[,] grid, Vector3 moveTo)
    {
        if (x>=0 && z>=0 && x<9 &&z<9)
        {
            if (x * 1.5 - moveTo.x < 0)
            {
                if (moveTo == grid[x + 1, z].transform.position)
                {
                    grid[x + 1, z].GetComponent<GridCell>().isOccupied = true;
                    return true;
                }
            }
            else if (x * 1.5 - moveTo.x > 0)
            {
                if (moveTo == grid[x - 1, z].transform.position)
                {
                    grid[x - 1, z].GetComponent<GridCell>().isOccupied = true;
                    return true;
                }
            }
            else if (z * 1.5 - moveTo.z < 0)
            {
                if (moveTo == grid[x, z + 1].transform.position)
                {
                    grid[x, z + 1].GetComponent<GridCell>().isOccupied = true;
                    return true;
                }
            }
            else if (z * 1.5 - moveTo.z > 0)
            {
                if (moveTo == grid[x, z - 1].transform.position)
                {
                    grid[x, z - 1].GetComponent<GridCell>().isOccupied = true;
                    return true;
                }
            }
        }
        return false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    private int height = 9;
    private int widht = 9;
    private float GridSpaceSize = 1.5f;

    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private GameObject boardPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject player;

    public GameObject[,] gameGrid;
    public Vector3[,] wallSnapPoints;
    public List<Vector3> snapPoints = new List<Vector3>();
    private GameObject gameBoard;
    private GameObject[] walls;
    

    void Awake()
    {
        CreateGrid();
        CreateBoard();
        CreateWalls();
        WallSnapPoints();
    }

    private void CreateGrid()
    {
        gameGrid = new GameObject[height, widht];
        if (gridCellPrefab == null)
        {
            Debug.LogError("gridCellPrefab not found");
            return;
        }

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < widht; x++)
            {
                gameGrid[x, z] = Instantiate(gridCellPrefab, new Vector3(x * GridSpaceSize, 0, z * GridSpaceSize), Quaternion.identity);
                if (z==0 && x == (widht-1)/2)
                {
                    player.GetComponent<Player>().SpawnPlayer(x,z,GridSpaceSize);
                    gameGrid[x, z].GetComponent<GridCell>().objectOnGridCell = player;
                    gameGrid[x, z].GetComponent<GridCell>().isOccupied = true;
                }
                gameGrid[x, z].GetComponent<GridCell>().SetPosition(x*GridSpaceSize, z*GridSpaceSize);
                gameGrid[x, z].transform.parent = transform;
                gameGrid[x, z].gameObject.name = "Grid space (X: " + x.ToString() + " Z: " + z.ToString() + ")";
            }
        }
    }

    private void CreateBoard()
    {
        if (boardPrefab == null)
        {
            Debug.LogError("boardPrefb not found");
            return;
        }

        gameBoard = Instantiate(boardPrefab, new Vector3((widht-1)*GridSpaceSize/2, 0, (height-1)*GridSpaceSize/2), Quaternion.identity);
        gameBoard.transform.parent = transform;
        gameBoard.transform.localScale = new Vector3(widht * GridSpaceSize,0.5f,height * GridSpaceSize);
        gameBoard.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
    }

    private void CreateWalls()
    {
        walls = new GameObject[10];
        if (wallPrefab == null)
        {
            return;
        }
        for (int i = 0; i < 10; i++)
        {
            walls[i] = Instantiate(wallPrefab, new Vector3(i*1.35f, 0, -3), Quaternion.identity);
            walls[i].GetComponent<Wall>().SetPosition(i*1.35f,-3);
        }
    }

    private void WallSnapPoints()
    {
        wallSnapPoints = new Vector3[height - 1, widht - 1];
        for (int z = 0; z < height-1; z++)
        {
            for (int x = 0; x < widht-1; x++)
            {
                wallSnapPoints[x, z] = new Vector3(x * GridSpaceSize + 0.75f,0.5f, z * GridSpaceSize + 0.75f);
                snapPoints.Add(new Vector3(x * GridSpaceSize + 0.75f, 0.5f, z * GridSpaceSize + 0.75f));
            }
        }
    }


    public Vector2Int GetGridPositionFromWorld(Vector3 worlPosition)
    {
        int x = Mathf.FloorToInt(worlPosition.x / GridSpaceSize);
        int z = Mathf.FloorToInt(worlPosition.z / GridSpaceSize);

        x = Mathf.Clamp(x, 0, widht);
        z = Mathf.Clamp(x, 0, height);

        return new Vector2Int(x, z);
    }

    public Vector3 GetWorldPositionFromGrid(Vector2Int gridPos)
    {
        float x = gridPos.x * GridSpaceSize;
        float z = gridPos.y * GridSpaceSize;

        return new Vector3(x, 0, z);
    }
}

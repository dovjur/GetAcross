using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{

    GameGrid gameGrid;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask whatIsAGridLayer;
    private GameObject selectedWall;
    private List<Vector3> snapPoints;
    public float snapRange = 0.5f;
    private List<Wall> walls;

    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
        snapPoints = gameGrid.GetComponent<GameGrid>().snapPoints;

        walls = FindObjectsOfType<Wall>().ToList();
    }

    void Update()
    {
        DragWall();
        GridCell cellMouseIsOver = IsMouseOverAGridCell();
        if (cellMouseIsOver != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cellMouseIsOver.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                player.GetComponentInChildren<Player>().MovePlayer(cellMouseIsOver.transform.position);
            }
        }
    }

    private GridCell IsMouseOverAGridCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out RaycastHit hitInfo, 100f ,whatIsAGridLayer))
        {
            return hitInfo.transform.GetComponent<GridCell>();
        }
        else
        {
            return null;
        }
    }
    private void DragWall()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedWall == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("Wall"))
                    {
                        return;
                    }
                    selectedWall = hit.collider.gameObject;
                    if (selectedWall.GetComponent<Wall>().isOnBoard)
                    {
                        selectedWall = null;
                        return;
                    }
                }
            }
            else
            {
                //MoveWall();
                snapPoints.Remove(selectedWall.transform.position);
                selectedWall.GetComponent<Wall>().isOnBoard = true;
                selectedWall = null;
                Cursor.visible = true;

            }
        }
        if (selectedWall != null)
        {
            MoveWall();

            if (Input.GetButtonDown("Rotate"))
            {
                selectedWall.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedWall.transform.rotation.eulerAngles.x,
                    selectedWall.transform.rotation.eulerAngles.y + 90,
                    selectedWall.transform.rotation.eulerAngles.z
                    )); ;
            }
            if (Input.GetMouseButtonDown(1))
            {
                selectedWall.transform.rotation = Quaternion.Euler(Vector3.zero);
                selectedWall.transform.position = selectedWall.GetComponent<Wall>().GetPosition();
                selectedWall = null;
                Cursor.visible = true;
            }
        }
    }
    private void MoveWall()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedWall.transform.position).z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        selectedWall.transform.position = new Vector3(worldPosition.x, 0.5f, worldPosition.z);
        foreach (Vector3 point in snapPoints)
        {
            float distance = Vector3.Distance(selectedWall.transform.position, point);
            if (distance < snapRange)
            {
                selectedWall.transform.position = point;
            }
        }
    }
    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }
}

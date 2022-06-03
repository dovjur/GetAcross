using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    GameGrid gameGrid;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask whatIsAGridLayer;
    // Start is called before the first frame update
    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
    }

    // Update is called once per frame
    void Update()
    {
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
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoSingleton<GridManager>
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cellSize;
    [SerializeField] private GameObject CellPrefab;
    public Node[,] Grid;

    [Header("Cell Settings")]
    public LayerMask cellLayerMask;

    public float CellSize { get => cellSize; set => cellSize = value; }
    public int GridWidth { get => gridWidth; set => gridWidth = value; }
    public int GridHeight { get => gridHeight; set => gridHeight = value; }

    public void Start()
    {
        CreateGrid();
        CenterCamera();
    }

    public void CreateGrid()
    {
        Grid = new Node[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);

               
                Vector3 worldPosition = new Vector3(
                    x * cellSize,
                    y * cellSize,
                    0);

                GameObject cell = Instantiate(CellPrefab, worldPosition, Quaternion.identity);

                cell.name = $"Cell {x},{y}";
                Grid[x, y] = new Node(position);
                Grid[x, y].WorldObject = cell; 
                cell.transform.parent = gameObject.transform;
            }
        }
    }

    // Calculate the center of the grid and place the camera in the center of the grid
    public void CenterCamera()
    {
        float m_gridWidth = gridWidth * cellSize;
        float m_gridHeight = gridHeight * cellSize;

        Vector3 gridCenter = new Vector3(m_gridWidth / 2f - cellSize / 2f, m_gridHeight / 2f - cellSize / 2f, -10f);
        Camera.main.transform.position = gridCenter;
    }

    // Checks whether the given position is within the borders and return Cell if position in borders.
    public Node GetGridCellByPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x >= 0 && gridPosition.x < GridWidth &&
            gridPosition.y >= 0 && gridPosition.y < GridHeight)
        {
            var cell = Grid[gridPosition.x, gridPosition.y];
            return cell;
        }
        else
        {
            return null;
        }
    }

    
}
public class Node
{
    public Vector2Int Position { get; set; }
    public GameObject WorldObject { get; set; } 
    public bool IsUsed{ get; set; }
    public Node(Vector2Int position)
    {
        Position = position;
    }
}

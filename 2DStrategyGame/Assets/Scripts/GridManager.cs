using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private float CellSize;
    [SerializeField] private float CellSpacing;
    [SerializeField] private GameObject CellPrefab;

    public Node[,] Grid;

    public void Start()
    {
        CreateGrid();
        CenterCamera();
    }
    public void CreateGrid()
    {
        Grid = new Node[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int position = new Vector2Int(x, y);

               
                Vector3 worldPosition = new Vector3(
                    x * (CellSize + CellSpacing),
                    y * (CellSize + CellSpacing),
                    0);

                GameObject cell = Instantiate(CellPrefab, worldPosition, Quaternion.identity);

                cell.name = $"Cell {x},{y}";
                Grid[x, y] = new Node(position);
                Grid[x, y].WorldObject = cell; // Node'un world object referansý
                cell.transform.parent = gameObject.transform;
            }
        }
    }

    public void CenterCamera()
    {
        float gridWidth = Width * (CellSize + CellSpacing);
        float gridHeight = Height * (CellSize + CellSpacing);

        // Gridin merkezini hesapla
        Vector3 gridCenter = new Vector3(gridWidth / 2f - CellSize / 2f, gridHeight / 2f - CellSize / 2f, -10f);

        // Kamerayý gridin merkezine yerleþtir
        Camera.main.transform.position = gridCenter;
    }
}
public class Node
{
    public Vector2Int Position { get; set; }
    public GameObject WorldObject { get; set; } 
    public Node(Vector2Int position)
    {
        Position = position;
    }
}

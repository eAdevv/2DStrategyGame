using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProductManager : MonoBehaviour,IProduct
{
    public ProductData productData;

    private int productWidth;
    private int productHeight;
    private GameObject buildObject;
    private bool isMouseBusy;
    private bool isPlaced;
    public virtual void Initialize()
    {
        transform.localScale = productData.Scale;
        productWidth = productData.Widht;
        productHeight = productData.Height;
        buildObject = productData.BuildObject;
        isMouseBusy = true;
    }
    private void Update()
    {
        if (!isPlaced)
        {
            #region Mouse Cursor Location

            Vector3 objectSize = GetComponent<Renderer>().bounds.size;
            transform.position +=  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)) - transform.position  + new Vector3(objectSize.x / 3f , objectSize.y / 3f , 0);

            #endregion

            #region Raycast Operation For Check Area & Object Placement
            // Checking Cell For Place Object
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, Mathf.Infinity, GridManager.Instance.cellLayerMask);

            if (hit.collider != null)
            {
                Vector2Int gridPosition = new Vector2Int(
                    Mathf.FloorToInt(hit.transform.position.x / GridManager.Instance.CellSize),
                    Mathf.FloorToInt(hit.transform.position.y / GridManager.Instance.CellSize)
                );

                // Check if the field to be placed is valid. If it is valid place gameObject and close are to reuse => AreaInactive().
                if (CanPlaceObject(gridPosition, productWidth, productHeight))
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    if (Input.GetMouseButtonDown(0) && isMouseBusy)
                    {
                        isMouseBusy = false;
                        isPlaced = true;
                        gameObject.transform.position = CalculateCenterPosition(gridPosition, productWidth, productHeight);
                        AreInactive(gridPosition, productWidth, productHeight);

                    }
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            #endregion
        }
        else
        {
            this.enabled = false;
        }


    }

    bool CanPlaceObject(Vector2Int startPosition, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int currentPosition = new Vector2Int(startPosition.x + x, startPosition.y + y);

                if (currentPosition.x >= 0 && currentPosition.x < GridManager.Instance.GridWidth &&
                    currentPosition.y >= 0 && currentPosition.y < GridManager.Instance.GridHeight)
                {
                    var cell = GridManager.Instance.Grid[currentPosition.x, currentPosition.y];
                    if (cell.IsUsed)
                    {
                        return false;
                    }
                }
            }
        }
        return true; 
    }


    void AreInactive(Vector2Int startPosition, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Calculate the current cell's position
                Vector2Int currentPosition = new Vector2Int(startPosition.x + x, startPosition.y + y);

                // Check if the current cell is within grid boundaries
                // If the cell is within the grid boundaries, turn off the cell's availability and set its color.
                if (currentPosition.x >= 0 && currentPosition.x < GridManager.Instance.GridWidth &&
                    currentPosition.y >= 0 && currentPosition.y < GridManager.Instance.GridHeight)
                {
                    var cell = GridManager.Instance.Grid[currentPosition.x, currentPosition.y];
                    cell.WorldObject.GetComponent<Collider2D>().enabled = false;
                    cell.IsUsed = true;
                    SpriteRenderer cellSpriteRenderer = cell.WorldObject.GetComponent<SpriteRenderer>();

                    if (cellSpriteRenderer != null)
                    {
                        Color color = cellSpriteRenderer.color;
                        color.a = 90 / 250f;
                        cellSpriteRenderer.color = color;
                    }
                }
            }
        }
    }

    Vector3 CalculateCenterPosition(Vector2Int startPosition, int width, int height)
    {
        // Calculate the grid coordinates in the center of the destroyed area and convert this position to WorldPosition.
        float centerX = startPosition.x + (width / 2f) - 0.5f;
        float centerY = startPosition.y + (height / 2f) - 0.5f;

        Vector3 centerWorldPosition = new Vector3(
            centerX * GridManager.Instance.CellSize,
            centerY * GridManager.Instance.CellSize,
            0
        );

        return centerWorldPosition;
    }


}
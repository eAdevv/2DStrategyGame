using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public abstract class Product : MonoBehaviour,IDamagable
{
    public ProductData productData;

    private Slider healthBar;
    private int productWidth;
    private int productHeight;

    private int productHealth;
    private int currentHealth;

    private GameObject buildObject;
    private bool isMouseBusy;
    private bool isPlaced;

    private List<Node> inactiveNodes = new List<Node>();
    public void Initialize()
    {
        transform.localScale = productData.Scale;
        productHealth = productData.Health;
        productWidth = productData.Widht;
        productHeight = productData.Height;

        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = productHealth;
        healthBar.value = productHealth;
        currentHealth = productHealth;

        isMouseBusy = true;
    }
    private void Update()
    {
        if (!isPlaced)
        {
            #region Mouse Cursor Location

            Vector3 objectSize = GetComponent<Renderer>().bounds.size;
            transform.position += Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)) - transform.position + new Vector3(objectSize.x / 3f, objectSize.y / 3f, 0);

            #endregion

            #region Raycast Operation For Check Area & Object Placement

            // Checking Cell For Place Object
            RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit(GridManager.Instance.cellLayerMask);

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
                    if (Input.GetMouseButtonUp(0) && isMouseBusy)
                    {
                        isMouseBusy = false;
                        isPlaced = true;

                        // Place Object 
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

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            foreach (Node cell in inactiveNodes.ToList())
            {
                cell.IsUsed = false;
                cell.WorldObject.GetComponent<Collider2D>().enabled = true;
                ColorAlphaChange(cell, 250);

            }

            Destroy(gameObject);

        }
    }

    bool CanPlaceObject(Vector2Int startPosition, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int currentPosition = new Vector2Int(startPosition.x + x, startPosition.y + y);

                var cell = GridManager.Instance.GetGridCellByPosition(currentPosition);

                if (cell == null || cell.IsUsed)
                {
                    return false;
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
                // If the cell is within the grid boundaries, turn off the cell's availability and set its color.
                Vector2Int currentPosition = new Vector2Int(startPosition.x + x, startPosition.y + y);
                var cell = GridManager.Instance.GetGridCellByPosition(currentPosition);

                if (cell != null)
                {
                    cell.WorldObject.GetComponent<Collider2D>().enabled = false;
                    cell.IsUsed = true;
                    ColorAlphaChange(cell, 90);
                    inactiveNodes.Add(cell);

                   
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

    void ColorAlphaChange(Node cell , int amount)
    {
        SpriteRenderer cellSpriteRenderer = cell.WorldObject.GetComponent<SpriteRenderer>();
        if (cellSpriteRenderer != null)
        {
            Color color = cellSpriteRenderer.color;
            color.a = amount / 250f;
            cellSpriteRenderer.color = color;
        }
    }


}

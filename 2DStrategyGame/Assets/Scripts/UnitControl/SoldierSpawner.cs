using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{

    private void OnEnable()
    {
        EventManager.OnSoldierSpawner += OnSoldierSpawner;
    }

    private void OnDisable()
    {
        EventManager.OnSoldierSpawner -= OnSoldierSpawner;
    }

    // Creating new soldier and Initializing its datas.
    public void OnSoldierSpawner(GameObject soldierObj, SoldierData data , Vector3 spawnPosition)
    {
        Soldier soldier = soldierObj.GetComponent<Soldier>();

        if (soldier != null)
        {
            soldier.Initialize(data.Health, data.ID, data.Damage, data.AttackRate);

            // Convert closest cell to spawn point ,according to Barrack's spawnPoint object position.
            Vector2Int closestPosition = GetClosestGridPosition(spawnPosition);
            SpawnSoldierOnGrid(closestPosition, soldierObj);
        }
        else
        {
            Debug.LogError("Soldier component not found on Prefab.");
        }
    }

    // Used to find the nearest grid cell
    // It does this by converting the world position to the grid position and adapting it to the grid dimensions.
    // Uses the spawn point inside the active Barrack object
    private Vector2Int GetClosestGridPosition(Vector3 position)
    {
        int gridX = Mathf.RoundToInt(position.x / GridManager.Instance.CellSize);
        int gridY = Mathf.RoundToInt(position.y / GridManager.Instance.CellSize);

        return new Vector2Int(gridX, gridY);
    }

    // Calculates the world position of the cell and creates the object in this position.
    // It takes the grid coordinate of the cell it creates and establishes its relationship with the grid cell.
    private void SpawnSoldierOnGrid(Vector2Int gridPosition, GameObject soldierObj)
    {
        var cell = GridManager.Instance.GetGridCellByPosition(gridPosition);

        if (cell != null)
        {
            Vector3 worldPos = new Vector3(
                 gridPosition.x * GridManager.Instance.CellSize,
                 gridPosition.y * GridManager.Instance.CellSize,
                 0
             );

            GameObject mySoldier = Instantiate(soldierObj, worldPos, Quaternion.identity);
            cell.IsUsed = true;
            mySoldier.transform.SetParent(cell.WorldObject.transform);
        }
        
    }
}

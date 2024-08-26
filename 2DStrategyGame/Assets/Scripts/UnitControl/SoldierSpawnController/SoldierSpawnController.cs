using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;
using System;

public class SoldierSpawnController : MonoSingleton<SoldierSpawnController>
{
    [SerializeField] private List<SoldierData> soldierDatas = new List<SoldierData>();
    [SerializeField] private List<GameObject> soldierPrefabs = new List<GameObject>();

    private BarrackProduct currentBarrackProduct;

    public BarrackProduct CurrentBarrackProduct { get => currentBarrackProduct; set => currentBarrackProduct = value; }

    public void CreateFirstSoldier()
    {
        Debug.Log("Test : First Soldier Create Operation.");
        EventManager.OnSoldierSpawner?.Invoke(soldierPrefabs[0], soldierDatas[0], currentBarrackProduct.spawnPoint.position);
    }

    public void CreateSecondSoldier()
    {
        Debug.Log("Test : Second Soldier Create Operation.");
        EventManager.OnSoldierSpawner?.Invoke(soldierPrefabs[1], soldierDatas[1], currentBarrackProduct.spawnPoint.position);
    }

    public void CreateThirdSoldier()
    {
        Debug.Log("Test : Third Soldier Create Operation.");
        EventManager.OnSoldierSpawner?.Invoke(soldierPrefabs[2], soldierDatas[2], currentBarrackProduct.spawnPoint.position);
    }

}

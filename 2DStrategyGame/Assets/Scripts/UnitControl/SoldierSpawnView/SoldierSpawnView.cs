using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierSpawnView : MonoBehaviour
{
    [SerializeField] private Button firstSoldierButton;
    [SerializeField] private Button secondSoldierButton;
    [SerializeField] private Button thirdSoldierButton;


    [SerializeField] private SoldierSpawnController soldierSpawnController;

    public void Awake()
    {
        firstSoldierButton.onClick.AddListener(soldierSpawnController.CreateFirstSoldier);
        secondSoldierButton.onClick.AddListener(soldierSpawnController.CreateSecondSoldier);
        thirdSoldierButton.onClick.AddListener(soldierSpawnController.CreateThirdSoldier);
    }

}

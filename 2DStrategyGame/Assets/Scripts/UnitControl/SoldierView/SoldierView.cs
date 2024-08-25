using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierView : MonoBehaviour
{
    [SerializeField] private Button firstSoldierButton;
    [SerializeField] private Button secondSoldierButton;
    [SerializeField] private Button thirdSoldierButton;


   [SerializeField] private SoldierController soldierController;


    public void Awake()
    {
        firstSoldierButton.onClick.AddListener(soldierController.CreateFirstSoldier);
        secondSoldierButton.onClick.AddListener(soldierController.CreateSecondSoldier);
        thirdSoldierButton.onClick.AddListener(soldierController.CreateThirdSoldier);
    }

   


}

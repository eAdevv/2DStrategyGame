using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInformationData", menuName = "ProductInformation/InformationData")]
public class ProductInformationData : ScriptableObject
{
    public string BuildName;
    public Sprite BuildIcon;
    public bool CanProduceUnit;
}

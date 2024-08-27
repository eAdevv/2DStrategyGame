using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductInformationModel : MonoBehaviour
{
    public ProductInformationData informationData;

    private string buildName;
    private Sprite buildIcon;
    private bool canPorduceUnit;

    public string BuildName { get => buildName; set => buildName = value; }
    public Sprite BuildIcon { get => buildIcon; set => buildIcon = value; }
    public bool CanPorduceUnit { get => canPorduceUnit; set => canPorduceUnit = value; }

    private void Awake()
    {
        if (informationData != null)
        {
            buildName = informationData.BuildName;
            buildIcon = informationData.BuildIcon;
        }
        else
        {
            Debug.LogError("Data not found.");
        }
    }

    public void SetInformation(string m_buildName, Sprite m_buildIcon, bool m_canProduce)
    {
        buildName = m_buildName;
        buildIcon = m_buildIcon;
        canPorduceUnit = m_canProduce;
        EventManager.OnSetBuildInformation?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductInformationController : MonoBehaviour
{
    private ProductInformationModel productInformation;
    private ProductInformationData productData;

    [SerializeField] private TextMeshProUGUI infoProductName;
    [SerializeField] private Image infoProductIcon;
    [SerializeField] private GameObject unitPanel;


    private void OnEnable()
    {
        EventManager.OnSetBuildInformation += OnUpdateInformationView;
    }

    private void OnDisable()
    {
        EventManager.OnSetBuildInformation -= OnUpdateInformationView;
    }

    private void Start()
    {
        infoProductIcon.gameObject.SetActive(false);
        infoProductName.gameObject.SetActive(false);
        unitPanel.SetActive(false);
    }


    private void OnUpdateInformationView()
    {
        if (productInformation == null) { return; }

        if (!infoProductName.isActiveAndEnabled) infoProductName.gameObject.SetActive(true);
        if (!infoProductIcon.isActiveAndEnabled) infoProductIcon.gameObject.SetActive(true);

        infoProductName.text = productInformation.BuildName;
        infoProductIcon.sprite = productInformation.BuildIcon;

        if (productInformation.CanPorduceUnit)
        {
            unitPanel.SetActive(true);
            SoldierSpawnController.Instance.CurrentBarrackProduct = productInformation.GetComponent<BarrackProduct>();
        }
        else unitPanel.SetActive(false);

    }

    public void OnGetInformation(ProductInformationModel m_ProductInformation)
    {
        productInformation = m_ProductInformation;
        productData = m_ProductInformation.informationData;
        productInformation.SetInformation(productData.BuildName, productData.BuildIcon, productData.CanProduceUnit);
    }

}

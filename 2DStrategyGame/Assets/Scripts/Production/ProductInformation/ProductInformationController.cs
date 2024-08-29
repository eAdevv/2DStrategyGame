using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProductInformationController : MonoBehaviour
{
    [Header("Model")]
    private ProductInformationModel productInformation;
    private ProductInformationData productData;

    [Header("View")]
    private ProductInformationView informationView;
    [SerializeField] private TextMeshProUGUI infoProductNameText;
    [SerializeField] private Image infoProductIcon;
    [SerializeField] private GameObject unitPanel;


    private void OnEnable()
    {
        EventManager.OnSetBuildInformation += OnUpdateInformationView;
        EventManager.OnCloseBuildInformation += CloseInformations;
    }

    private void OnDisable()
    {
        EventManager.OnSetBuildInformation -= OnUpdateInformationView;
        EventManager.OnCloseBuildInformation -= CloseInformations;
    }

    private void Awake()
    {
        if (GetComponent<ProductInformationView>() != null) informationView = GetComponent<ProductInformationView>();
        else
        {
            Debug.LogError("No Information View component found.");
            return;
        }
    }
    private void Start()
    {
        infoProductIcon.gameObject.SetActive(false);
        infoProductNameText.gameObject.SetActive(false);
        unitPanel.SetActive(false);
    }

    private void OnUpdateInformationView()
    {
        if (productInformation == null) { return; }


        if (!informationView.InformationPanel.activeInHierarchy) informationView.InformationPanel.SetActive(true);

        if (!infoProductNameText.isActiveAndEnabled && !infoProductIcon.isActiveAndEnabled)
        {
            infoProductNameText.gameObject.SetActive(true);
            infoProductIcon.gameObject.SetActive(true);
        }


        infoProductNameText.text = productInformation.BuildName;
        infoProductIcon.sprite = productInformation.BuildIcon;

        if (productInformation.CanPorduceUnit)
        {
            unitPanel.SetActive(true);
            SoldierSpawnController.Instance.CurrentBarrackProduct = productInformation.GetComponent<BarrackProduct>();
        }
        else
            unitPanel.SetActive(false);
    }

    private void CloseInformations()
    {
        informationView.InformationPanel.SetActive(false);
        infoProductNameText.text = null;
        infoProductIcon.sprite = null;
    }


    public void OnGetInformation(ProductInformationModel m_ProductInformation)
    {
        productInformation = m_ProductInformation;
        productData = m_ProductInformation.informationData;
        productInformation.SetInformation(productData.BuildName, productData.BuildIcon, productData.CanProduceUnit);
    }

}

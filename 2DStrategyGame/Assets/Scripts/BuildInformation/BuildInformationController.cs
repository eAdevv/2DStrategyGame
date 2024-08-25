using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildInformationController : MonoBehaviour
{
    private BuildInformationModel buildInformation;

    [SerializeField] private TextMeshProUGUI infoBuildName;
    [SerializeField] private Image infoBuildIcon;
    [SerializeField] private GameObject unitPanel;
    private BuildInformationData buildData;

  
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
        infoBuildIcon.gameObject.SetActive(false);
        infoBuildName.gameObject.SetActive(false);
        unitPanel.SetActive(false);
    }


    private void OnUpdateInformationView()
    {
        if (buildInformation == null) { return; }

        if (!infoBuildName.isActiveAndEnabled) infoBuildName.gameObject.SetActive(true);
        if (!infoBuildIcon.isActiveAndEnabled) infoBuildIcon.gameObject.SetActive(true);

        infoBuildName.text = buildInformation.BuildName;
        infoBuildIcon.sprite = buildInformation.BuildIcon;

        if (buildInformation.CanPorduceUnit)
        {
            unitPanel.SetActive(true);
            SoldierController.Instance.CurrentBarrackProduct = buildInformation.GetComponent<BarrackProduct>();
        }
        else unitPanel.SetActive(false);

    }

    public void OnGetInformation(BuildInformationModel m_buildInformation)
    {
        buildInformation = m_buildInformation;
        buildData = m_buildInformation.informationData;
        buildInformation.SetInformation(buildData.BuildName, buildData.BuildIcon , buildData.CanProduceUnit);     
    }

   


}

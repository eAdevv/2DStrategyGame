using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInformationView : MonoBehaviour
{
    [SerializeField] private LayerMask buildLayerMask;
    private BuildInformationController informationController;

    private void Start()
    {
        informationController = GetComponent<BuildInformationController>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit(buildLayerMask);

            if (hit.collider != null)
            {
                informationController.OnGetInformation(hit.collider.GetComponent<BuildInformationModel>());
            }
            else return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductInformationView : MonoBehaviour
{
    [SerializeField] private LayerMask buildLayerMask;
    private ProductInformationController informationController;

    private void Start()
    {
        informationController = GetComponent<ProductInformationController>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit(buildLayerMask);

            if (hit.collider != null)
            {
                informationController.OnGetInformation(hit.collider.GetComponent<ProductInformationModel>());
            }
            else return;
        }
    }
}

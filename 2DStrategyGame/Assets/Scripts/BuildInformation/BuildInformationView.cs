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
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, Mathf.Infinity, buildLayerMask);

            if (hit.collider != null)
            {
                informationController.OnGetInformation(hit.collider.GetComponent<BuildInformationModel>());
            }
            else return;
        }
    }
}

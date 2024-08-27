using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierInteractManager : MonoBehaviour
{
    private Soldier managedSoldier;

    public LayerMask soldierMask;
    public LayerMask InteractMask;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit(soldierMask);

            if (hit.collider != null)
            {
                managedSoldier = hit.collider.GetComponent<Soldier>();
            }
        }
        if (Input.GetMouseButtonUp(1) && managedSoldier != null)
        {
            RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit(InteractMask);

            if (hit.collider != null)
            {
                Vector2Int targetPosition = GridManager.Instance.GetGridWorldPosition(hit.collider.transform.position);
                managedSoldier.Move(targetPosition);
            }

        }

    }

   
}

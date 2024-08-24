using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductionMenuManager : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    const float OUT_OF_BOUNDS_THRESHOLD = 40f;
    const float CHILD_WEIGHT = 125f;
    const float CHILD_HEIGHT = 125f;
    const float ITEM_SPACÝNG = 15f;

    private Vector2 lastDragPosition;
    private bool isPositiveDrag;
    private int childCount;
    private float height;

    ScrollRect scrollRect;
    GridLayoutGroup gridLayoutGroup;

    [SerializeField] private GameObject[] productionList;
    private void Awake()
    {
        if (GetComponent<ScrollRect>() != null)
            scrollRect = GetComponent<ScrollRect>();
        else
            Debug.LogError("Scroll Rect missing.");
    }
    private void Start()
    {
        for (int i = 0; i < productionList.Length; i++)
        {
            PoolManager.Instance.CreatePool(productionList[i].ToString(), productionList[i], 1);
            PoolManager.Instance.GetObject(productionList[i].ToString(), transform.position, Quaternion.identity);
        }

        childCount = scrollRect.content.childCount;
        gridLayoutGroup = scrollRect.content.GetComponent<GridLayoutGroup>();
        height = Screen.height;
    }

    void OnEnable()
    {
        scrollRect.onValueChanged.AddListener(ItemSwitcher);
    }

    void OnDisable()
    {
        scrollRect.onValueChanged.RemoveListener(ItemSwitcher);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPosition = eventData.position;
        isPositiveDrag = newPosition.y > lastDragPosition.y;
        lastDragPosition = newPosition;
    }

    bool ReachedThreshold(Transform item)
    {
        float up_Threshold = transform.position.y + height * 0.5f + OUT_OF_BOUNDS_THRESHOLD;
        float down_Threshold = transform.position.y - height * 0.5f - OUT_OF_BOUNDS_THRESHOLD;

        if (isPositiveDrag)
            return item.position.y - CHILD_WEIGHT * 0.5f > up_Threshold;
        else
            return item.position.y + CHILD_WEIGHT * 0.5f < down_Threshold;
    }

    void ItemSwitcher(Vector2 value)
    {
        if (gridLayoutGroup.IsActive()) gridLayoutGroup.enabled = false;

        int currentItemIndex = isPositiveDrag ? childCount - 1 : 0;
        var currentItem = scrollRect.content.GetChild(currentItemIndex);

        if (!ReachedThreshold(currentItem)) return;

        int lastItemIndex = isPositiveDrag ? 0 : childCount - 1;
        var lastItem = scrollRect.content.GetChild(lastItemIndex);
        Vector2 newPosition = lastItem.position;


        if (isPositiveDrag)
            newPosition.y = lastItem.position.y - 125f * 1.5f + ITEM_SPACÝNG;
        else
            newPosition.y = lastItem.position.y + 125f * 1.5f - ITEM_SPACÝNG;


        PoolManager.Instance.ReturnObjectToQueue(currentItem.gameObject);
        GameObject newItem = PoolManager.Instance.GetObject(currentItem.GetComponent<PoolObject>().poolKey, newPosition, Quaternion.identity);

        newItem.transform.SetSiblingIndex(lastItemIndex);
    }


}

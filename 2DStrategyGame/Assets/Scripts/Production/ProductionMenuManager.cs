
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductionMenuManager : MonoSingleton<ProductionMenuManager>, IBeginDragHandler, IDragHandler
{
    const float OUT_OF_BOUNDS_THRESHOLD = 0.5f;
    const float CHILD_HEIGHT = 125f;
    const float ITEM_SPACÝNG = 12f;

    private Vector2 lastDragPosition;
    private bool isPositiveDrag;
    private int childCount;
    private float screenHeight;
    private bool isMouseBusy;

    ScrollRect scrollRect;
    GridLayoutGroup gridLayoutGroup;

    [SerializeField] private GameObject[] productionList;
    [SerializeField] private GameObject viewPort;

    public bool IsMouseBusy { get => isMouseBusy; set => isMouseBusy = value; }

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
        screenHeight = Screen.height;
       
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

    // Checks the position of the object in the scene according to the specified threshold value.
    bool ReachedThreshold(Transform item)
    {
        float up_Threshold = viewPort.transform.position.y + screenHeight * OUT_OF_BOUNDS_THRESHOLD;
        float down_Threshold = viewPort.transform.position.y - screenHeight * OUT_OF_BOUNDS_THRESHOLD;

        if (isPositiveDrag)
            return item.position.y - CHILD_HEIGHT * 0.5f > up_Threshold;
        else
            return item.position.y + CHILD_HEIGHT * 0.5f < down_Threshold;
    }


    // According to the direction of interaction, it deactivates the top object from the pool and recreates it at the bottom.
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
            newPosition.y = lastItem.position.y - 125 * 1.5f + ITEM_SPACÝNG;
        else
            newPosition.y = lastItem.position.y + 125 * 1.5f - ITEM_SPACÝNG;


        PoolManager.Instance.ReturnObjectToQueue(currentItem.gameObject);
        GameObject newItem = PoolManager.Instance.GetObject(currentItem.GetComponent<PoolObject>().poolKey, newPosition, Quaternion.identity);

        newItem.transform.SetSiblingIndex(lastItemIndex);
    }


}

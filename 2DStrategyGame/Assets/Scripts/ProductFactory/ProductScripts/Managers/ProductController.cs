using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ProductController : MonoBehaviour
{
    [SerializeField] public ProductFactoryHolder factoryData;
    private ProductFactory myFactory;

    private void Start()
    {
        myFactory = factoryData.ProductFactory;
        GetComponent<Button>().onClick.AddListener(GetProduct);
    }
    private void GetProduct()
    {
        // Produce the product from the factory in mouse position.
        Vector3 myPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        myFactory.CreateProduct(myPosition);
    }

    
}

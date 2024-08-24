using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFactory", menuName = "Production/ProductFactory")]
public class ProductFactoryHolder : ScriptableObject
{
    public ProductFactory ProductFactory;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProductData", menuName = "Production/ProductData")]
public class ProductData : ScriptableObject
{
    public Vector3 Scale;
    public int Widht;
    public int Height;
    public int Health;
}

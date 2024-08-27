using UnityEngine;

public abstract class ProductFactory : MonoBehaviour
{
    public abstract Product CreateProduct(Vector2 position);
    
}

using UnityEngine;

public abstract class ProductFactory : MonoBehaviour
{
    public abstract IProduct CreateProduct(Vector2 position);
    
}

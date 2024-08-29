using UnityEngine;

public class BarnFactory : ProductFactory
{
    [SerializeField] private BarnProduct _barnProduct;
    public override Product CreateProduct(Vector2 position)
    {
        GameObject barnObject = Instantiate(_barnProduct.gameObject, position, Quaternion.identity);
        BarnProduct newBarn = barnObject.GetComponent<BarnProduct>();
        newBarn.Initialize();

        return newBarn;
    }
}

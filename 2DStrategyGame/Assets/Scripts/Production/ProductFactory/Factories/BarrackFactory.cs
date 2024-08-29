using UnityEngine;

public class BarrackFactory : ProductFactory
{
    [SerializeField] private BarrackProduct _barrackProduct;
    public override Product CreateProduct(Vector2 position)
    {
        GameObject barrackObject = Instantiate(_barrackProduct.gameObject, position, Quaternion.identity);
        BarrackProduct newBarrack = barrackObject.GetComponent<BarrackProduct>();

        newBarrack.Initialize();
        return newBarrack;
    }
}

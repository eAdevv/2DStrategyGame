using UnityEngine;

public class PowerPlantFactory : ProductFactory
{
    [SerializeField] private PowerPlantProduct _libraryProduct;
    public override IProduct CreateProduct(Vector2 position)
    {
        GameObject powerPlantObject = Instantiate(_libraryProduct.gameObject, position, Quaternion.identity);
        PowerPlantProduct newPowerPlant = powerPlantObject.GetComponent<PowerPlantProduct>();

        newPowerPlant.Initialize();

        return newPowerPlant;
    }
}

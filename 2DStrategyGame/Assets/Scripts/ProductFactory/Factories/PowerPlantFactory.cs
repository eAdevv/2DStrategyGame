using UnityEngine;

public class PowerPlantFactory : ProductFactory
{
    private PowerPlantProduct _libraryProduct;
    public override Product CreateProduct(Vector2 position)
    {
        GameObject powerPlantObject = Instantiate(_libraryProduct.gameObject, position, Quaternion.identity);
        PowerPlantProduct newPowerPlant = powerPlantObject.GetComponent<PowerPlantProduct>();

        newPowerPlant.Initialize();

        return newPowerPlant;
    }
}

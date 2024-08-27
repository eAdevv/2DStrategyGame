using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentFactory : ProductFactory
{
    private TentProduct _tentProduct;
    public override Product CreateProduct(Vector2 position)
    {
        GameObject tentObject = Instantiate(_tentProduct.gameObject, position, Quaternion.identity);
        TentProduct newTent = tentObject.GetComponent<TentProduct>();

        newTent.Initialize();

        return newTent;
    }
}

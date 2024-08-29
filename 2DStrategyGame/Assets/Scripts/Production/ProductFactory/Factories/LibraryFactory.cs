using UnityEngine;

public class LibraryFactory : ProductFactory
{
    [SerializeField] private LibraryProduct _libraryProduct;
    public override Product CreateProduct(Vector2 position)
    {
        GameObject libraryObject = Instantiate(_libraryProduct.gameObject, position, Quaternion.identity);
        LibraryProduct newLibrary = libraryObject.GetComponent<LibraryProduct>();

        newLibrary.Initialize();
        return newLibrary;
    }
}

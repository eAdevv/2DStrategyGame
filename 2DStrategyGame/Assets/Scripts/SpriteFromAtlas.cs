
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public class SpriteFromAtlas : MonoBehaviour
{
    [SerializeField] SpriteAtlas atlas;
    [SerializeField] string spriteName;

    private void Start()
    {
        GetComponent<Image>().sprite = atlas.GetSprite(spriteName);
    }
}

using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public int itemCode;
    public int goldValue;

    public Sprite itemIcon;
    public string itemDescription;
}

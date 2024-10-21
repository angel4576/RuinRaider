using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public int price;
    public int count;

    public ItemData(string itemName, int count)
    {
        this.itemName = itemName;
        this.count = count;
    }
}
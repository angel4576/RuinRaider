using UnityEngine.UI;
using TMPro;
using UnityEngine;
//using UnityEditorInternal;

public class ShopSlot : ItemSlot
{
    public TextMeshProUGUI price;
    public Image goldIcon;

    public override void AddItem(ItemData item)
    {
        base.AddItem(item);
        price.text = item.price.ToString();
        goldIcon.enabled = true;
    }

    public override void ClearItem()
    {
        base.ClearItem();
        AudioManager.Instance.PlayMusic("discover", false);
        price.text = "";
        goldIcon.enabled = false;
    }

    public void BuyItem()
    {
        AudioManager.Instance.PlayMusic("click", false);
        if(InventoryManager.Instance.gold > item.price)
        {
            InventoryManager.Instance.AddGold(-item.price);
            InventoryManager.Instance.AddItem(item);
            Debug.Log(item);
            ClearItem();
        }
        else
            Debug.Log("not enough gold.");
    }
}

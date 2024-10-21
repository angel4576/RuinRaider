using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public GameObject player;
    public ItemData item;
    public Image icon;
    public TextMeshProUGUI countText;

    public virtual void AddItem(ItemData item)
    {
        this.item = item;
        icon.sprite = item.icon;
        icon.enabled = true;
        countText.text = item.count.ToString();
    }

    public virtual void ClearItem()
    {
        this.item = null;
        icon.sprite = null;
        icon.enabled = false;
        countText.text = "";
    }

    public void UseItem()
    {
        AudioManager.Instance.PlayMusic("click", false);
        if(item.itemName != "Boss Key") {
            bool canUse = InventoryManager.Instance.UseItem(item.itemName);
            if(!canUse) {
                Debug.Log("ItemSlot -> UseItem -> Inventory Manager returned false");
            }
        }
    }
}

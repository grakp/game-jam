using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public Item item;
    public Image itemImage;

    public void InitializeSlot(Item newItem)
    {
        item = newItem;
        if (itemImage != null)
        {
            itemImage.sprite = item.image;
        }

        // instantiate the InventoryItem prefab and initialize it
        GameObject newItemGo = Instantiate(InventoryManager.instance.inventoryItemPrefab, transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    // testing
    public Item[] startItems;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (var item in startItems)
        {
            AddItem(item);
        }
    }

    public void AddItem(Item item)
    {
        // find empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            Debug.Log($"Slot {i}: {(itemInSlot == null ? "Empty" : "Occupied")}");
            if (itemInSlot == null) {
                Debug.Log($"Adding item to slot {i}");
                SpawnNewItem(item, slot);
                return;
            }
        }
        Debug.Log("No empty slots available");
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
}

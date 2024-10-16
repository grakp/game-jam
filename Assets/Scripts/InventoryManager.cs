using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<InventorySlot> inventorySlots;
    public GameObject inventoryItemPrefab;
    private List<Item> collectedItems = new List<Item>();

    // testing
    public Item[] possibleItems;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the sceneLoaded event
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReinitializeInventorySlots();
        UpdateInventoryUI(); // update the inventory UI when a new scene is loaded
    }

    private void ReinitializeInventorySlots()
    {
        // Find all inventory slots in the new scene
        inventorySlots = new List<InventorySlot>(FindObjectsOfType<InventorySlot>());
    }

   public void AddItem(Item item)
    {
        collectedItems.Add(item);
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public void UpdateInventoryUI()
    {
        // collect items to be destroyed
        List<GameObject> itemsToDestroy = new List<GameObject>();

        // collect existing items in slots
        foreach (var slot in inventorySlots)
        {
            foreach (Transform child in slot.transform)
            {
                itemsToDestroy.Add(child.gameObject);
            }
        }

        // destroy collected items
        foreach (var item in itemsToDestroy.ToArray()) // use ToArray() to create a copy
        {
            Destroy(item);
        }

        // add collected items back to slots
        foreach (var item in collectedItems.ToArray())
        {
            AddItem(item);
        }
    }
}

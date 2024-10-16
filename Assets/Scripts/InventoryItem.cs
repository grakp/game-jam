using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour
{
    [HideInInspector] public Item item;

    [Header("UI")]
    public UnityEngine.UI.Image itemImage;
    public void Start()
    {
        InitializeItem(item);
    }

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        if (itemImage != null)
        {
            itemImage.sprite = newItem.image;
        }
        else
        {
            Debug.LogWarning("ItemImage is not assigned in the InventoryItem script.");
        }
        
    }
}

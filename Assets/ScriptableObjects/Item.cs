using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item")] // for testing
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite image;
    
}

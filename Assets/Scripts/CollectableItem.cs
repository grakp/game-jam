using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public int itemIndex;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    public float scaleFactor = 1.1f;

    private void Start()
    {
        originalScale = transform.localScale;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Collectable"))
                {
                    Debug.Log("hit collectable");
                    Collect();
                }
            }
        }
    }

    private void Collect()
    {
        Item item = InventoryManager.instance.possibleItems[itemIndex];
        InventoryManager.instance.AddItem(item);
        gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse entered " + gameObject.name);
        transform.localScale = originalScale * scaleFactor;
        Debug.Log("Transform scale set to: " + transform.localScale);
        
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse exited " + gameObject.name);
        transform.localScale = originalScale;
    }
}

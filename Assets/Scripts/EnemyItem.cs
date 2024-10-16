using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyItem : MonoBehaviour
{
    public int itemIndex;
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
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("hit enemy");
                    LoadFightScene(hit.collider.gameObject);
                }
            }
        }
    }

    private void LoadFightScene(GameObject enemy)
{
    string sceneName = EnemyManager.instance.GetFightSceneForEnemy(enemy);
    if (!string.IsNullOrEmpty(sceneName))
    {
        SceneManager.LoadScene(sceneName);
    }
    else
    {
        Debug.LogError("No fight scene found for the enemy: " + enemy.name);
    }
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

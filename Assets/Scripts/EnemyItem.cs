using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyItem : MonoBehaviour
{
    public int enemyIndex;
    public float scaleFactor = 1.1f;

    private Vector3 originalScale;
    private DialogueController dialogueController;
    private bool dialogueTriggered = false;
    
    private void Start()
    {
        originalScale = transform.localScale;
        dialogueController = FindObjectOfType<DialogueController>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !dialogueTriggered)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                EnemyItem clickedEnemy = hit.collider.GetComponent<EnemyItem>();

                if (clickedEnemy != null && clickedEnemy == this)
                {
                    Debug.Log("Hit enemy: " + gameObject.name + " with enemyIndex: " + enemyIndex);
                    GameStateManager.instance.SavePlayerState(gameObject);
                    dialogueTriggered = true;

                    if (enemyIndex == 2) // boss
                    {
                        dialogueController.PlayDialogues(0, 3);
                        dialogueController.postDialogueAction = () => 
                        {
                            SceneManager.LoadScene("BossFight");
                        };
                    }
                    else
                    {
                        TriggerDialogueAndLoadFightScene();
                    }   
                }
            }
        }
    }

    private void TriggerDialogueAndLoadFightScene()
    {
        if (dialogueController != null)
        {
            // trigger the dialogue and pass a callback to load the fight scene after the dialogue finishes
            dialogueController.TriggerDialogueForEnemy(enemyIndex, () => StartCoroutine(LoadFightScene(enemyIndex)));            
        }
    }

    private IEnumerator LoadFightScene(int enemyIndex)
    {
        string sceneName = EnemyManager.instance.GetFightSceneForEnemy(enemyIndex);
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Loading fight scene: " + sceneName);
            yield return null;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("No fight scene found for the enemy: " + enemyIndex);
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

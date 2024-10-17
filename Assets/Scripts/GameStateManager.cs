using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    private Vector3 playerPosition;
    private List<Item> playerInventory = new List<Item>();
    private string previousSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerState(GameObject player)
    {
        playerPosition = player.transform.position;
        previousSceneName = SceneManager.GetActiveScene().name;
    }

    public void RestorePlayerState(GameObject player)
    {
        player.transform.position = playerPosition;
    }

    public string GetPreviousSceneName()
    {
        return previousSceneName;
    }
}

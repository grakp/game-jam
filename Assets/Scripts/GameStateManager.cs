using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    private Vector3 playerPosition;
    private string previousSceneName;
    private bool isFirstLoad = true;

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

    public bool IsFirstLoad()
    {
        return isFirstLoad;
    }

    public void SetFirstLoad(bool value)
    {
        isFirstLoad = value;
    }
}

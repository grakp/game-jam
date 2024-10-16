using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<GameObject> possibleEnemies;
    public List<string> fightScenes;

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

    public string GetFightSceneForEnemy(GameObject enemy)
    {
        int index = possibleEnemies.IndexOf(enemy);
        Debug.Log("Enemy: " + enemy.name + ", Index: " + index); // Add debug log
        if (index >= 0 && index < fightScenes.Count)
        {
            return fightScenes[index];
        }
        return null;
    }
}

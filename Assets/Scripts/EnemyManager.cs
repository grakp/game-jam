using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<Enemy> possibleEnemies;
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

    public string GetFightSceneForEnemy(int enemyIndex)
    {
        if (enemyIndex >= 0 && enemyIndex < possibleEnemies.Count)
        {
            return possibleEnemies[enemyIndex].fightScene;
        }
        return null;
    }
}

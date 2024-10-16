using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;
    public string fightScene;
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<EnemyData> enemiesData;
}

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public float currentHealth;
}

[System.Serializable]
public class EnemyData
{
    public string enemyId; 
    public Vector3 position;
    public float currentHealth;
    public bool isDead;
}
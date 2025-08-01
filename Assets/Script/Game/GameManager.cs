using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private PlayerHeath playerHeath;
    [SerializeField] private PlayerMovement playerMovement;

    private bool isGameOver = false;

    void Start()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            TriggerSaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            TriggerLoadGame();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void TriggerSaveGame()
    {
        GameData data = new GameData();
        data.playerData = new PlayerData()
        {
            position = playerHeath.transform.position,
            currentHealth = playerHeath.currentHeath
        };
        data.enemiesData = new List<EnemyData>();
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (var enemy in enemies)
        {
            EnemyIdentifier identifier = enemy.GetComponent<EnemyIdentifier>();
            if (identifier != null)
            {
                data.enemiesData.Add(new EnemyData()
                {
                    enemyId = identifier.id,
                    position = enemy.transform.position,
                    currentHealth = enemy.currentHealth,
                    isDead = enemy.currentHealth <= 0
                });
            }
        }

        SaveManager.Instance.SaveGame(data);
    }

    public void TriggerLoadGame()
    {
        GameData data = SaveManager.Instance.LoadGame();
        if (data == null) return;
        playerHeath.transform.position = data.playerData.position;
        playerHeath.LoadHealth(data.playerData.currentHealth);
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (var enemy in enemies)
        {
            EnemyIdentifier identifier = enemy.GetComponent<EnemyIdentifier>();
            if (identifier != null)
            {
                foreach (var enemyData in data.enemiesData)
                {
                    if (enemyData.enemyId == identifier.id)
                    {
                        if (enemyData.isDead)
                        {
                            Destroy(enemy.gameObject);
                        }
                        else
                        {
                            enemy.transform.position = enemyData.position;
                            enemy.LoadHealth(enemyData.currentHealth);
                        }
                        break;
                    }
                }
            }
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel; 
    [SerializeField] private  PlayerHeath playerHeath; 
    [SerializeField] private  PlayerMovement playerMovement; 

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
}
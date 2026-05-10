using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Make sure game over panel is hidden at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    
    public void TriggerGameOver(string winnerName)
    {
        Debug.Log($"GAME OVER! Winner: {winnerName}");
        
        // Freeze the game
        Time.timeScale = 0f;
        
        // Display winner text
        if (winnerText != null)
        {
            winnerText.text = $"{winnerName}\nWINS!";
        }
        
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    
    public void RestartGame()
    {
        // Unpause
        Time.timeScale = 1f;
        
        // Reload the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    public void QuitToMenu()
    {
        // Unpause
        Time.timeScale = 1f;
        
        // Load main menu (you'll need to create this scene)
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
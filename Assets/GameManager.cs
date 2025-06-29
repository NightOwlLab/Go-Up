using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;

    [Header("Game Settings")]
    public float fallThreshold = -5.0f;
    public bool saveHighScore = true;

    private float highestPoint = 0f;
    private bool isGameOver = false;
    private int currentScore = 0;
    private int highScore = 0;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        LoadHighScore();
        InitializeUI();
    }

    private void InitializeUI()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        UpdateScoreDisplay();
    }

    private void LoadHighScore()
    {
        if (saveHighScore)
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }
    }

    private void SaveHighScore()
    {
        if (saveHighScore && currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if (!isGameOver && player != null)
        {
            UpdateScore();
            CheckGameOver();
        }
    }

    private void UpdateScore()
    {
        // Calculate score based on highest point reached
        if (player.position.y > highestPoint)
        {
            highestPoint = player.position.y;
            currentScore = Mathf.FloorToInt(highestPoint);
            UpdateScoreDisplay();
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
            if (saveHighScore && highScore > 0)
            {
                scoreText.text += $"\nHigh Score: {highScore}";
            }
        }
    }

    private void CheckGameOver()
    {
        // Check if player has fallen too far below their highest point
        if (player.position.y < highestPoint + fallThreshold)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls
        
        isGameOver = true;
        SaveHighScore();
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (pauseButton != null)
            pauseButton.SetActive(false);
            
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        if (isGameOver) return;
        
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return highScore;
    }
}

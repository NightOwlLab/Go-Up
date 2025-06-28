using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;

    private float highestPoint = 0f;
    private bool isGameOver = false;
    public float fallThreshold = - 5.0f;

    void Start()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Hitung skor berdasarkan ketinggian tertinggi
            if (player.position.y > highestPoint)
            {
                highestPoint = player.position.y;
                scoreText.text = "Score: " + Mathf.FloorToInt(highestPoint).ToString();
            }

            if (!gameOverPanel.activeInHierarchy && player.position.y < highestPoint + fallThreshold)
            {
                GameOver();
            }

        }
    }

    void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);
        // Opsional: berhenti update physics
        Time.timeScale = 0;
    }

    // Tombol ulang
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

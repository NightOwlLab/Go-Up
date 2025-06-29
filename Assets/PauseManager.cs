using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseScreen;

    private bool isPaused = false;

    public void PauseGame()
    {
        if (pauseScreen == null)
        {
            Debug.LogWarning("PauseManager: Pause screen is not assigned!");
            return;
        }

        if (isPaused) return; // Already paused

        pauseScreen.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseScreen == null)
        {
            Debug.LogWarning("PauseManager: Pause screen is not assigned!");
            return;
        }

        if (!isPaused) return; // Not paused

        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    void Update()
    {
        // ESC key for pause/unpause on PC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Android back button support
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        // Auto-pause when app loses focus on mobile
        if (pauseStatus && !isPaused)
        {
            PauseGame();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Auto-pause when app loses focus
        if (!hasFocus && !isPaused)
        {
            PauseGame();
        }
    }
}

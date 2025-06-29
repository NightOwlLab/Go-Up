using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startScreen;
    public GameObject pauseButton;
    
    [Header("Game References")]
    public GameObject player;

    private bool gameStarted = false;
    private Player playerComponent;

    void Start()
    {
        InitializeComponents();
        SetupStartState();
    }

    private void InitializeComponents()
    {
        if (startScreen == null)
        {
            Debug.LogError("StartScreenManager: Start screen is not assigned!");
            return;
        }

        if (player != null)
        {
            playerComponent = player.GetComponent<Player>();
            if (playerComponent == null)
            {
                Debug.LogError("StartScreenManager: Player component not found on player GameObject!");
            }
        }
        else
        {
            Debug.LogError("StartScreenManager: Player GameObject is not assigned!");
        }
    }

    private void SetupStartState()
    {
        if (startScreen != null)
            startScreen.SetActive(true);

        if (pauseButton != null)
            pauseButton.SetActive(false);

        // Disable player movement at start
        if (playerComponent != null)
        {
            playerComponent.SetMovementEnabled(false);
        }
    }

    void Update()
    {
        if (!gameStarted)
        {
            CheckForGameStart();
        }
    }

    private void CheckForGameStart()
    {
        // Check for touch/click input to start game
        bool inputDetected = false;

        // Touch input for mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputDetected = true;
        }
        // Mouse input for desktop
        else if (Input.GetMouseButtonDown(0))
        {
            inputDetected = true;
        }
        // Keyboard input (Space or Enter)
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            inputDetected = true;
        }

        if (inputDetected)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (gameStarted) return; // Prevent multiple calls

        gameStarted = true;

        // Hide start screen
        if (startScreen != null)
            startScreen.SetActive(false);

        // Show pause button
        if (pauseButton != null)
            pauseButton.SetActive(true);

        // Enable player movement
        if (playerComponent != null)
        {
            playerComponent.SetMovementEnabled(true);
        }

        Debug.Log("Game started!");
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public void ResetStartScreen()
    {
        gameStarted = false;
        SetupStartState();
    }
}

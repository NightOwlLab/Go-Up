using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    public GameObject startScreen; // Panel UI yang berisi tombol dan petunjuk\
    public GameObject player;
    private bool gameStarted = false;
    public GameObject pauseButton;

    void Start()
    {
        startScreen.SetActive(true);
        // Nonaktifkan pergerakan pemain saat awal
        if (player != null)
        {
            player.GetComponent<Player>().enabled = false;
            pauseButton.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            startScreen.SetActive(false); // Hilangkan panel start screen
            pauseButton.SetActive(true);
            gameStarted = true;

            // Aktifkan pergerakan pemain
            if (player != null)
            {
                player.GetComponent<Player>().enabled = true;
            }
        }
    }
}

using UnityEngine;

public class SimplePauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas; // Reference to the PauseMenuCanvas
    public AudioSource musicSource; // Reference to the music
    public AudioClip recordScratch; // Record scratch sound effect
    private bool isPaused = false;

    void Update()
    {
        // Toggle pause menu when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Play record scratch sound
        if (recordScratch != null)
        {
            AudioSource.PlayClipAtPoint(recordScratch, Vector3.zero);
        }

        // Show pause menu
        pauseMenuCanvas.SetActive(true);

        // Pause music
        if (musicSource != null)
        {
            musicSource.Pause();
        }

        // Pause the game
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        // Hide pause menu
        Debug.Log("Resume button clicked");
        pauseMenuCanvas.SetActive(false);

        // Resume music
        if (musicSource != null)
        {
            musicSource.UnPause();
        }

        // Resume the game
        Time.timeScale = 1;
        isPaused = false;
    }

    public void QuitGame()
    {
        // Resume time before quitting
        Time.timeScale = 1;
        Application.Quit();
        Debug.Log("Game Quit");
    }
}

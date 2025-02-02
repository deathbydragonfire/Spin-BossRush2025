using UnityEngine;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject lorePanel;
    public GameObject tutorialPanel;
    public GameObject mainMenuPanel;

    void Start()
    {
        // Ensure only the lore panel is visible at the start
        lorePanel.SetActive(true);
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }

    public void ContinueFromLore()
    {
        lorePanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void ContinueFromTutorial()
    {
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        Debug.Log("Starting Game...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("newHannaScene"); // Replace with actual scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}

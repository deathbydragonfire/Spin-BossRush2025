using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreenHandler : MonoBehaviour
{
    public CanvasGroup deathScreenCanvasGroup; // Attach the CanvasGroup for fading
    public float fadeDuration = 2f; // Time to fully fade to black

    private bool isFading = false;

    public void TriggerDeathScreen()
    {
        if (!isFading)
        {
            StartCoroutine(FadeToBlack());
        }
    }

    private IEnumerator FadeToBlack()
    {
        isFading = true;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            deathScreenCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }

        deathScreenCanvasGroup.alpha = 1; // Ensure fully black
        isFading = false;

        // Enable interaction on the death screen
        deathScreenCanvasGroup.interactable = true;
        deathScreenCanvasGroup.blocksRaycasts = true;
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current level
    }
}
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WordManager : MonoBehaviour
{
    public GameObject wordPrefab; // Prefab for word objects
    public Transform wordSpawnArea; // Area where words can spawn
    public TMP_InputField typingInput; // The invisible input field
    private List<Word> activeWords = new List<Word>();
    private Word currentWord = null;

    private string[] wordPool = { "apple", "banana", "cherry", "dragon", "elephant" };

    void Start()
    {
        // Ensure the input field is ready for input
        typingInput.ActivateInputField();

        // Spawn multiple words at the start
        for (int i = 0; i < 3; i++)
        {
            SpawnWord();
        }
    }

    void Update()
    {
        // Keep the input field focused
        if (!typingInput.isFocused)
        {
            typingInput.ActivateInputField();
        }

        // Handle each key typed
        foreach (char c in Input.inputString)
        {
            HandleInput(c);
        }
    }

    void HandleInput(char typedLetter)
    {
        if (currentWord == null)
        {
            // Find the word that starts with the typed letter
            foreach (Word word in activeWords)
            {
                if (word.StartsWithLetter(typedLetter))
                {
                    currentWord = word;
                    break;
                }
            }
        }

        // If a word is active, pass the letter to it
        if (currentWord != null)
        {
            if (currentWord.TypeLetter(typedLetter))
            {
                // Word completed
                activeWords.Remove(currentWord);
                currentWord = null;
                SpawnWord(); // Spawn a new word
            }
        }

        // Clear the input field text
        typingInput.text = "";
    }

    public void SpawnWord()
    {
        RectTransform spawnRect = wordSpawnArea.GetComponent<RectTransform>();
        Vector2 spawnAreaMin = spawnRect.offsetMin;
        Vector2 spawnAreaMax = spawnRect.offsetMax;

        Vector2 spawnPosition;
        bool positionIsValid;

        do
        {
            positionIsValid = true;

            // Generate a random position
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            spawnPosition = new Vector2(randomX, randomY);

            // Check if the position overlaps with existing words
            foreach (Word word in activeWords)
            {
                RectTransform wordRects = word.GetComponent<RectTransform>();
                Vector2 existingPosition = wordRects.anchoredPosition;

                float distance = Vector2.Distance(spawnPosition, existingPosition);
                if (distance < 100) // Adjust this value for minimum spacing
                {
                    positionIsValid = false;
                    break;
                }
            }
        } while (!positionIsValid);

        // Spawn the word at the valid position
        GameObject newWordObj = Instantiate(wordPrefab, wordSpawnArea);
        Word newWord = newWordObj.GetComponent<Word>();
        newWord.SetWord(wordPool[Random.Range(0, wordPool.Length)]);

        // Set position
        RectTransform wordRect = newWordObj.GetComponent<RectTransform>();
        wordRect.anchoredPosition = spawnPosition;

        // Add the word to the active list
        activeWords.Add(newWord);
    }


}

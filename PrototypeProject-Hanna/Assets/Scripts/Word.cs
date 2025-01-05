using UnityEngine;
using TMPro;

public class Word : MonoBehaviour
{
    public TMP_Text wordText;
    private string fullWord;
    private string currentTyped;

    public void SetWord(string word)
    {
        fullWord = word;
        currentTyped = "";
        wordText.text = fullWord;
    }

    public bool TypeLetter(char letter)
    {
        if (fullWord[currentTyped.Length] == letter)
        {
            currentTyped += letter;
            wordText.text = $"<color=green>{currentTyped}</color>{fullWord.Substring(currentTyped.Length)}";

            if (currentTyped == fullWord)
            {
                Destroy(gameObject); // Word is fully typed
                return true;
            }
        }
        return false;
    }

    public bool StartsWithLetter(char letter)
    {
        return fullWord[0] == letter;
    }
}

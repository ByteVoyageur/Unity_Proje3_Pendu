using System;
using UnityEngine;
using UnityEngine.UIElements;

public class WordGenerator : MonoBehaviour
{
    public string[] words = new string[] { "FRANCE", "ORANGE", "PHONE", "LIBERTE" };
    private System.Random random = new System.Random();

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Label wordLabel = root.Q<Label>("WordLabel");
        
        string randomWord = GetRandomWord();
        wordLabel.text = randomWord;
        
        // Optional: Debug log to confirm the word is set
        Debug.Log($"Selected Word: {randomWord}");
    }

    string GetRandomWord()
    {
        int index = random.Next(words.Length);
        return words[index];
    }
}
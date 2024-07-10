using System.Collections;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

// This class fetches a random word from an API and displays it on the UI
public class WordGenerator : MonoBehaviour
{
    public string apiUrl = "https://trouve-mot.fr/api/random"; // API URL
    private WordMatcher wordMatcher;

    void OnEnable()
    {
        wordMatcher = GetComponent<WordMatcher>();
        StartCoroutine(GetRandomWordFromAPI());
    }

//fetch a random word from the API
    public IEnumerator GetRandomWordFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // Send a request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching word: {webRequest.error}");
            }
            else
            {
                // Retrieve data from the response
                string jsonResult = webRequest.downloadHandler.text;

                // Parse the JSON data
                ApiResponse[] words = JsonHelper.FromJson<ApiResponse>(jsonResult);

                if (words != null && words.Length > 0)
                {
                    // Get the name of the first word
                    string randomWord = words[0].name.ToUpper(); // Convert to uppercase

                    SetWordToLabel(randomWord);
                    wordMatcher.Initialize(NormalizeString(randomWord)); // Initialize the WordMatcher with normalized word
                }
                else
                {
                    Debug.LogWarning("No words found in the API response.");
                }
            }
        }
    }

// Set the word to the UI label
    void SetWordToLabel(string word)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Label wordLabel = root.Q<Label>("WordLabel");

        wordLabel.text = word;

        Debug.Log($"Selected Word: {word}");
    }

// Normalize the word to remove diacritics and convert to uppercase
    string NormalizeString(string input)
    {
        string normalizedString = input.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToUpper(); // Ensure the string is uppercase
    }

// Represents the data structure of the API response
    [System.Serializable]
    public class ApiResponse
    {
        public string name;
        public string categorie;
    }

// Helper class for handling JSON data
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            ApiResponseWrapper<T> wrapper = JsonUtility.FromJson<ApiResponseWrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class ApiResponseWrapper<T>
        {
            public T[] array;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class WordGenerator : MonoBehaviour
{
    public string apiUrl = "https://trouve-mot.fr/api/random"; // API URL
    private WordMatcher wordMatcher;

    void OnEnable()
    {
        wordMatcher = GetComponent<WordMatcher>();
        StartCoroutine(GetRandomWordFromAPI());
    }

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
                    wordMatcher.Initialize(randomWord); // Initialize the WordMatcher after setting the word
                }
                else
                {
                    Debug.LogWarning("No words found in the API response.");
                }
            }
        }
    }

    void SetWordToLabel(string word)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Label wordLabel = root.Q<Label>("WordLabel");

        wordLabel.text = word;

        // Optional: Debug log to confirm the word is set
        Debug.Log($"Selected Word: {word}");
    }

    [System.Serializable]
    public class ApiResponse
    {
        public string name;
        public string categorie;
    }

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

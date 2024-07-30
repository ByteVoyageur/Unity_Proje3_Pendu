using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace Pendu.GameStart{
public class WordGenerator : MonoBehaviour
{
    public string apiUrl = "https://trouve-mot.fr/api/random"; // API URL
    private WordMatcher wordMatcher;
    private Button nextButton;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        wordMatcher = GetComponent<WordMatcher>();
        nextButton = root.Q<Button>("next-button");
        nextButton.clicked += OnNextButtonClicked;

        // Disable the Next button initially
        nextButton.SetEnabled(false);
        
        StartCoroutine(GetRandomWordFromAPI());
    }

    private void OnNextButtonClicked()
    {
        StartCoroutine(GetRandomWordFromAPI());
    }

    public IEnumerator GetRandomWordFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching word: {webRequest.error}");
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;
                ApiResponse[] words = JsonHelper.FromJson<ApiResponse>(jsonResult);

                if (words != null && words.Length > 0)
                {
                    string randomWord = words[0].name.ToUpper();
                    string category = words[0].categorie;

                    Debug.Log($"Random Word: {randomWord}");
                    SetWordAndCategoryToLabel(randomWord, category);
                    wordMatcher.Initialize(randomWord, WordNormalizer.NormalizeString(randomWord));
                    
                    // Disable the Next button when initializing word
                    nextButton.SetEnabled(false);
                }
                else
                {
                    Debug.LogWarning("No words found in the API response.");
                }
            }
        }
    }

    void SetWordAndCategoryToLabel(string word, string category)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Label wordLabel = root.Q<Label>("WordLabel");
        Label hintCategoryLabel = root.Q<Label>("HintCategory");

        string underscoreWord = new string(' ', word.Length);

        wordLabel.text = underscoreWord;
        hintCategoryLabel.text = category;
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
}

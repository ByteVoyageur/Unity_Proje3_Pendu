using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneController : MonoBehaviour
{
    private VisualElement root;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component is missing.");
            return;
        }

        root = uiDocument.rootVisualElement;

        var playButton = root.Q<Button>("PLAY");
        if (playButton == null)
        {
            Debug.LogError("Button with name 'PLAY' not found.");
            return;
        }

        playButton.clicked += OnPlayButtonClicked;
    }

    void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    
}

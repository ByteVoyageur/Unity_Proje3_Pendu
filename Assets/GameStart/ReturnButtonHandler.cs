using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReturnButtonHandler : MonoBehaviour
{
    public GameObject menuPage;
    public GameObject gameStartPage;
    public GameObject settingsPage;
    private Button returnButton;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        returnButton = root.Q<Button>("return-button");
        if (returnButton != null)
        {
            returnButton.clicked += OnReturnClicked;
        }
        else
        {
            Debug.LogError("No Button with name 'return-button' found!");
        }
    }

    public void OnReturnClicked()
    {
        menuPage.SetActive(true);
        gameStartPage.SetActive(false);
        settingsPage.SetActive(false); 
    }
}
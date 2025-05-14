using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pendu.GameStart{
    /// <summary>
    /// Handles the functionality of the "Return" button, including switching between different UI pages.
    /// </summary>
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
/// <summary>
/// Event handler for when the 'return' button is clicked. It switches to the menu page.
/// and hides the game start and settings pages.
/// </summary>
    public void OnReturnClicked()
    {
        menuPage.SetActive(true);
        gameStartPage.SetActive(false);
        settingsPage.SetActive(false); 
    }
}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReturnButtonHandler : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject gameStart;
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
        gameMenu.SetActive(true);
        gameStart.SetActive(false); 
    }
}

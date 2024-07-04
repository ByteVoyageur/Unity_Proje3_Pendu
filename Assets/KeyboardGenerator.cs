using UnityEngine;
using UnityEngine.UIElements;

public class KeyboardGenerator : MonoBehaviour
{
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement keyboardContainer = root.Q<VisualElement>("InputLettre");

        char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        foreach (char letter in alphabet)
        {
            Button button = new Button() { text = letter.ToString() };
            button.name = letter.ToString().ToLower();
            button.AddToClassList(letter.ToString().ToLower());
            keyboardContainer.Add(button);
            
            // Debug log to confirm the class is added
        }
    }
}
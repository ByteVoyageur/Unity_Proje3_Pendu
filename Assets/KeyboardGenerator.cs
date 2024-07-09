using UnityEngine;
using UnityEngine.UIElements;

public class KeyboardGenerator : MonoBehaviour
{
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement keyboardContainer = root.Q<VisualElement>("KeyboardButtons");

        char[] alphabet = "AZERTYUIOPQSDFGHJKLMWXCVBN".ToCharArray();

        foreach (char letter in alphabet)
        {
            Button button = new Button() { text = letter.ToString() };
            button.name = letter.ToString().ToLower();
            button.AddToClassList("keyboard-button");  // use a same class name
            keyboardContainer.Add(button);
        }
    }
}
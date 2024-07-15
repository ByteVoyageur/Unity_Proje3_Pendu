using UnityEngine;
using UnityEngine.UIElements;

public class HangmanAnimator : MonoBehaviour
{
    private VisualElement[] hangmanParts;
    private int currentPartIndex = 0;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Initialize hangman parts
        hangmanParts = new VisualElement[10];
        hangmanParts[0] = root.Q<VisualElement>("Base");
        hangmanParts[1] = root.Q<VisualElement>("VerticalPole");
        hangmanParts[2] = root.Q<VisualElement>("Support");
        hangmanParts[3] = root.Q<VisualElement>("Rope");
        hangmanParts[4] = root.Q<VisualElement>("Head");
        hangmanParts[5] = root.Q<VisualElement>("Body");
        hangmanParts[6] = root.Q<VisualElement>("LeftArm");
        hangmanParts[7] = root.Q<VisualElement>("RightArm");
        hangmanParts[8] = root.Q<VisualElement>("LeftLeg");
        hangmanParts[9] = root.Q<VisualElement>("RightLeg");

        ResetAnimation();
    }

    public void ResetAnimation()
    {
        currentPartIndex = 0;
        foreach (var part in hangmanParts)
        {
            part.style.display = DisplayStyle.None;
        }
    }

    public void ShowNextPart()
    {
        if (currentPartIndex < hangmanParts.Length)
        {
            hangmanParts[currentPartIndex].style.display = DisplayStyle.Flex;
            currentPartIndex++;
        }
    }
}

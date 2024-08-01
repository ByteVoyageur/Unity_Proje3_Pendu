using UnityEngine;
using UnityEngine.UIElements;

namespace Pendu.GameStart{
public class HangmanAnimator : MonoBehaviour
{
    private VisualElement[] hangmanParts;
    private int currentPartIndex = 0;

    // Public reference to SoundManager
    public SoundManager soundManager;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        soundManager = FindObjectOfType<SoundManager>();

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
            if (part != null)
            {
                string partName = part.name.ToLower();
                part.RemoveFromClassList($"{partName}-visible");
                part.style.display = DisplayStyle.None;
            }
        }
    }

    public void ShowNextPart()
    {
        if (currentPartIndex < hangmanParts.Length)
        {
            var part = hangmanParts[currentPartIndex];
            if (part != null)
            {
                string partName = part.name.ToLower();
                part.style.display = DisplayStyle.Flex;
                part.AddToClassList($"{partName}-visible");

                // Ensure soundManager instance is assigned and then call PlayGrowUpSound()
                if (soundManager != null)
                {
                    soundManager.PlayGrowUpSound();
                }
                else
                {
                    Debug.LogWarning("SoundManager is not assigned in the HangmanAnimator script.");
                }

                currentPartIndex++;
            }
        }
    }
}
}
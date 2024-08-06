using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pendu.GameStart{
    /// <summary>
    /// Provides helper methods for applying animations to UI elements , specifically
    /// animations for letters that have been matched in the word guessing game.
    /// </summary>
public static class AnimationHelper
{
    public static void ApplyMatchedLetterAnimations(Label wordLabel, MonoBehaviour monoBehaviour)
    {
        
        foreach (var element in wordLabel.Children())
        {
            if (element.ClassListContains("matched-letter"))
            {
                monoBehaviour.StartCoroutine(AnimateElement(element));
            }
        }
    }

    private static IEnumerator AnimateElement(VisualElement element)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 originalScale = element.transform.scale;
        Vector3 targetScale = originalScale * 1.5f;


        // Scaling up
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            element.transform.scale = Vector3.Lerp(originalScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset scale after the animation
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            element.transform.scale = Vector3.Lerp(targetScale, originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        element.transform.scale = originalScale; // Ensure it's reset
    }
}
}
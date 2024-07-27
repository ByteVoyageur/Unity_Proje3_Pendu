using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public static class AnimationHelper
{
    public static void ApplyMatchedLetterAnimations(Label wordLabel, MonoBehaviour monoBehaviour)
    {
        Debug.Log("Applying matched letter animations...");
        
        foreach (var element in wordLabel.Children())
        {
            if (element.ClassListContains("matched-letter"))
            {
                Debug.Log($"Animating element: {element.name}");
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

        Debug.Log("Starting animation...");

        // Scaling up
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            element.transform.scale = Vector3.Lerp(originalScale, targetScale, t);
            Debug.Log($"Scaling up: {element.transform.scale}");
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
        Debug.Log("Animation complete.");
    }
}
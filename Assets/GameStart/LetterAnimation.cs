using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public static class LetterAnimation
{
    public static void PlayAnimation(VisualElement parent, Label targetLabel, Action onComplete)
    {
        // Create a temporary label to animate
        var tempLabel = new Label { text = targetLabel.text };
        tempLabel.style.position = Position.Absolute;
        tempLabel.style.left = targetLabel.worldBound.xMin;
        tempLabel.style.top = targetLabel.worldBound.yMin;
        tempLabel.style.fontSize = targetLabel.style.fontSize;
        tempLabel.style.color = targetLabel.style.color;
        tempLabel.style.unityFont = targetLabel.style.unityFont;
        tempLabel.style.opacity = 1f;

        parent.Add(tempLabel);

        // Animation sequence with 1 second duration
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(DOTween.To(() => 1f, x => tempLabel.style.scale = new Scale(new Vector3(x, x, 1)), 1.5f, 0.5f)); // Scale up duration to 0.5 seconds
        animationSequence.Append(DOTween.To(() => 1.5f, x => tempLabel.style.scale = new Scale(new Vector3(x, x, 1)), 1f, 0.5f)); // Scale down duration to 0.5 seconds
        animationSequence.OnComplete(() =>
        {
            tempLabel.RemoveFromHierarchy();
            onComplete?.Invoke();
        });
    }

    public static void UpdateLabelWithAnimation(VisualElement parent, string text, Color color, float x, float y)
    {
        var label = new Label { text = text };
        label.style.position = Position.Absolute;
        label.style.left = x;
        label.style.top = y;
        label.style.fontSize = 72;
        label.style.color = new StyleColor(color);
        label.style.opacity = 1f;

        parent.Add(label);

        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(DOTween.To(() => 1f, v => label.style.scale = new Scale(new Vector3(v, v, 1)), 2f, 0.5f)); // Scale up duration to 0.5 seconds
        animationSequence.Append(DOTween.To(() => 2f, v => label.style.scale = new Scale(new Vector3(v, v, 1)), 1f, 0.5f)); // Scale down duration to 0.5 seconds
        animationSequence.OnComplete(() => label.RemoveFromHierarchy());
    }
}

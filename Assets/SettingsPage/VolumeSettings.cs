using UnityEngine;
using UnityEngine.UIElements;

public class VolumeSettings : MonoBehaviour
{
    public SoundManager soundManager; // 引用 SoundManager 脚本
    private Slider volumeSlider;
    private Label volumeLabel;

    void OnEnable()
    {
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not assigned.");
            return;
        }

        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found!");
            return;
        }

        var root = uiDocument.rootVisualElement;

        volumeSlider = root.Q<Slider>("SlideOne");
        volumeLabel = root.Q<Label>("VolumeValue");

        if (volumeSlider == null)
        {
            Debug.LogError("Slider not found in UXML.");
            return;
        }

        if (volumeLabel == null)
        {
            Debug.LogError("Label not found in UXML.");
            return;
        }

        volumeSlider.value = soundManager.normalClickSound.volume * 100; 
        volumeLabel.text = "Volume: " + Mathf.RoundToInt(volumeSlider.value).ToString();

        volumeSlider.RegisterValueChangedCallback(evt =>
        {
            SetVolume(evt.newValue);
            soundManager.PlayNormalClickSound();
        });
    }

    void SetVolume(float volume)
    {
        if (soundManager != null)
        {
            soundManager.SetVolume(volume / 100f); 
            volumeLabel.text = "Volume: " + Mathf.RoundToInt(volume).ToString();
        }
    }

    void OnDisable()
    {
        if (volumeSlider != null)
        {
            volumeSlider.UnregisterValueChangedCallback(evt => SetVolume(evt.newValue));
        }
    }
}

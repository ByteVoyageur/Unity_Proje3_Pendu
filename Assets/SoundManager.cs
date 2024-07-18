using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip winSound;
    public AudioClip loseSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWinSound()
    {
        Debug.Log("Playing win sound");
        audioSource.clip = winSound;
        audioSource.Play();
    }

    public void PlayLoseSound()
    {
        Debug.Log("Playing lose sound");
        audioSource.clip = loseSound;
        audioSource.Play();
    }
}
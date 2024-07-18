using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip nomalClickSound;
    public AudioClip errorClickSound;

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

    public void PlayNomalClickSound ()
    {
        audioSource.clip= nomalClickSound;
        audioSource.Play();
    }

    public void PlayErrorClickSound ()
    {
        audioSource.clip= errorClickSound;
        audioSource.Play();
    }
}